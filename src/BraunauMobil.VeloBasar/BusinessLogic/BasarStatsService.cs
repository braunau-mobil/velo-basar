using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xan.Extensions;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public class BasarStatsService
    : IBasarStatsService
{
    private readonly IColorProvider _colorProvider;
    private readonly VeloDbContext _db;
    private readonly IFormatProvider _formatProvider;
    private readonly ApplicationSettings _settings;
    private readonly ISellerService _sellerService;

    public BasarStatsService(IColorProvider colorProvider, VeloDbContext db, IFormatProvider formatProvider, IOptions<ApplicationSettings> settings, ISellerService sellerService)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _colorProvider = colorProvider ?? throw new ArgumentNullException(nameof(colorProvider));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
        _settings = settings.Value;
        _sellerService = sellerService ?? throw new ArgumentNullException(nameof(sellerService));
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

    public async Task<BasarSettlementStatus> GetSettlementStatusAsync(BasarEntity basar)
    {
        ArgumentNullException.ThrowIfNull(basar);

        if (basar.State == ObjectState.Disabled)
        {
            return new BasarSettlementStatus(false, 0, 0, 0);
        }

        int settlementCount = await _db.Transactions.AsNoTracking()
            .WhereBasarAndType(basar.Id, TransactionType.Settlement)
            .CountAsync();
        if (settlementCount == 0)
        {
            return new BasarSettlementStatus(false, 0, 0, 0);
        }

        SellerListParameter sellerListParameter = new()
        {
            BasarId = basar.Id,
            PageSize = int.MaxValue,
            State = ObjectState.Enabled,
            ValueState = ValueState.NotSettled
        };
        IPaginatedList<CrudItemModel<SellerEntity>> sellers = await _sellerService.GetManyAsync(sellerListParameter);
        int onSiteCount = sellers.Count(item => item.Entity.SettlementType == SellerSettlementType.OnSite);
        int remoteCount = sellers.Count(item => item.Entity.SettlementType == SellerSettlementType.Remote);

        int overallNotSettledCount = await _db.Sellers.Where(seller => seller.State == ObjectState.Enabled && seller.ValueState == ValueState.NotSettled).CountAsync();

        return new BasarSettlementStatus(true, overallNotSettledCount, onSiteCount, remoteCount);
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
