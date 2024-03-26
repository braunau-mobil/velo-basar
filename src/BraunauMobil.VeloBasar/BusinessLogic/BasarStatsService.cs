using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public class BasarStatsService
    : IBasarStatsService
{
    private readonly IColorProvider _colorProvider;
    private readonly VeloDbContext _db;
    private readonly IFormatProvider _formatProvider;
    private readonly ApplicationSettings _settings;

    public BasarStatsService(IColorProvider colorProvider, VeloDbContext db, IFormatProvider formatProvider, IOptions<ApplicationSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _colorProvider = colorProvider ?? throw new ArgumentNullException(nameof(colorProvider));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
        _settings = settings.Value;
    }

    public async Task<int> GetAcceptanceCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .WhereBasarAndType(basarId, TransactionType.Acceptance)
            .CountAsync();

    public async Task<IReadOnlyList<ProductEntity>> GetAcceptedProductsAsync(int basarId)
        => await _db.Products.AsNoTracking()
            .Include(product => product.Session)
            .Include(product => product.Type)
            .AsNoTracking()
            .Where(product => product.Session.BasarId == basarId && product.StorageState != StorageState.NotAccepted)
            .ToArrayAsync();

    public decimal GetAcceptedProductsAmount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(nameof(products));

        return products.Where(product => product.StorageState != StorageState.NotAccepted).SumPrice();
    }

    public int GetAcceptedProductsCount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Where(product => product.StorageState != StorageState.NotAccepted).Count();
    }

    public IReadOnlyList<ChartDataPoint> GetAcceptedProductTypesWithAmount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return GroupByTypeAndAmount(products);
    }

    public IReadOnlyList<ChartDataPoint> GetAcceptedProductTypesWithCount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return GroupByTypeAndCount(products);
    }

    public int GetLockedProductsCount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Where(product => product.StorageState == StorageState.Locked).Count();
    }

    public int GetLostProductsCount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Where(product => product.StorageState == StorageState.Lost).Count();
    }

    public IReadOnlyList<ChartDataPoint> GetPriceDistribution(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        IEnumerable<decimal> productPrices = products.Select(product => decimal.Round(product.Price, 2)).ToArray();

        if (!productPrices.Any())
        {
            return [];
        }

        List<ChartDataPoint> data = [];
        foreach (PriceRange range in _settings.PriceDistributionRanges)
        {
            int count = productPrices.Count(range.IsInRange);
            string label = range.GetLabel(_formatProvider);

            data.Add(new ChartDataPoint(count, label, _colorProvider[label]));
        }
        return data.ToArray();
    }

    public async Task<int> GetSaleCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .WhereBasarAndType(basarId, TransactionType.Sale)
            .CountAsync();

    public IReadOnlyList<ChartDataPoint> GetSaleDistribution(IEnumerable<Tuple<TimeOnly, decimal>> transactionsAndTotals)
    {
        ArgumentNullException.ThrowIfNull(transactionsAndTotals);

        return transactionsAndTotals
            .OrderBy(x => x.Item1)
            .GroupBy(x => new TimeOnly(x.Item1.Hour, x.Item1.Minute))
            .Select(g => new ChartDataPoint(g.Sum(x => x.Item2), g.Key.ToString("t", _formatProvider), _colorProvider.Primary))
            .ToArray();
    }

    public async Task<int> GetSellerCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .WhereBasarAndType(basarId, TransactionType.Acceptance)
            .Select(transaction => transaction.SellerId)
            .Distinct()
            .CountAsync();

    public async Task<BasarSettlementStatus> GetSettlementStatusAsync(int basarId)
    {
        var products = await _db.Products
            .Include(p => p.Session)
                .ThenInclude(s => s.Seller)
            .Where(p => p.Session.BasarId == basarId)
            .Select(p => new
            {
                Product = p,
                Seller = new
                {
                    p.Session.SellerId,
                    p.Session.Seller.ValueState,
                    p.Session.Seller.IBAN
                }
            })
            .AsNoTracking()
            .ToArrayAsync();

        int overallSettledCount = 0;
        int overallNotSettledCount = 0;
        int mayBeCount = 0;
        int mustBeCount = 0;
        foreach (var group in products.GroupBy(p => p.Seller))
        {
            if (group.Key.ValueState == ValueState.Settled)
            {
                overallSettledCount++;
            }
            else
            {
                overallNotSettledCount++;

                if (group.Key.IBAN is not null && group.Where(p => p.Product.ValueState == ValueState.NotSettled).All(x => x.Product.CanBeSettledWithoutSeller))
                {
                    mayBeCount++;
                }
                else
                {
                    mustBeCount++;
                }
            }
        }

        return new BasarSettlementStatus(
            overallSettledCount > 0,
            overallSettledCount,
            overallNotSettledCount,
            mustBeCount,
            mayBeCount            
        );
    }

    public async Task<IReadOnlyList<Tuple<TimeOnly, decimal>>> GetSoldProductTimestampsAndPricesAsync(int basarId)
    {
        return await (from transaction in _db.Transactions
                      join productToTransaction in _db.ProductToTransaction on transaction.Id equals productToTransaction.TransactionId
                      join product in _db.Products on productToTransaction.ProductId equals product.Id
                      where transaction.BasarId == basarId && transaction.Type == TransactionType.Sale && product.StorageState == StorageState.Sold
                      select new Tuple<TimeOnly, decimal>(TimeOnly.FromDateTime(transaction.TimeStamp), product.Price))
            .AsNoTracking()
            .ToArrayAsync();
    }

    public decimal GetSoldProductsAmount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return GetSoldProducts(products).SumPrice();
    }

    public int GetSoldProductsCount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return GetSoldProducts(products).Count();
    }

    public IReadOnlyList<ChartDataPoint> GetSoldProductTypesWithAmount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return GroupByTypeAndAmount(GetSoldProducts(products));
    }

    public IReadOnlyList<ChartDataPoint> GetSoldProductTypesWithCount(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return GroupByTypeAndCount(GetSoldProducts(products));
    }

    private IReadOnlyList<ChartDataPoint> GroupByTypeAndAmount(IEnumerable<ProductEntity> products)
    {
        var productTypeAmounts = products
            .OrderBy(product => product.Type.Name)
            .GroupBy(product => product.Type.Name)
            .Select(group => new
            {
                Name = group.Key,
                Amount = group.SumPrice()
            });

        return productTypeAmounts
            .Select(x => new ChartDataPoint(x.Amount, x.Name, _colorProvider[x.Name]))
            .ToArray();
    }

    private IReadOnlyList<ChartDataPoint> GroupByTypeAndCount(IEnumerable<ProductEntity> products)
    {
        var productTypeAmounts = products
            .OrderBy(product => product.Type.Name)
            .GroupBy(product => product.Type.Name)
            .Select(group => new
            {
                Name = group.Key,
                Count = group.Count()
            });

        return productTypeAmounts
            .Select(x => new ChartDataPoint(x.Count, x.Name, _colorProvider[x.Name]))
            .ToArray();
    }

    private static IEnumerable<ProductEntity> GetSoldProducts(IEnumerable<ProductEntity> products)
    {
        return products.Where(product => product.StorageState == StorageState.Sold);
    }
}
