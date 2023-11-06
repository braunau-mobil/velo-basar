using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public class BasarStatsService
    : IBasarStatsService
{
    private readonly IColorProvider _colorProvider;
    private readonly VeloDbContext _db;

    public BasarStatsService(IColorProvider colorProvider, VeloDbContext db)
    {
        _colorProvider = colorProvider ?? throw new ArgumentNullException(nameof(colorProvider));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<int> GetAcceptanceCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .ForBasar(basarId, TransactionType.Acceptance)
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

        IEnumerable<decimal> productPrices = products.Select(product => product.Price).ToArray();

        if (!productPrices.Any())
        {
            return Array.Empty<ChartDataPoint>();
        }

        decimal step = 10.0m;
        decimal maxPrice = productPrices.Max();
        decimal currentMin = 0.0m;
        decimal currentMax = Math.Min(step, maxPrice);

        List<ChartDataPoint> data = new();
        while (currentMax < maxPrice)
        {
            int count = productPrices.Count(price => price >= currentMin && price < currentMax);
            if (count > 0)
            {
                string label = $"{currentMax:C}";
                Color color = _colorProvider.Primary;
                data.Add(new ChartDataPoint(count, label, color));
            }
            currentMin += step;
            currentMax += step;
        }
        return data.ToArray();
    }

    public async Task<int> GetSaleCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .ForBasar(basarId, TransactionType.Sale)
            .CountAsync();

    public IReadOnlyList<ChartDataPoint> GetSaleDistribution(IEnumerable<Tuple<TimeOnly, decimal>> transactionsAndTotals)
    {
        ArgumentNullException.ThrowIfNull(transactionsAndTotals);

        return transactionsAndTotals.GroupBy(x => new TimeOnly(x.Item1.Hour, x.Item1.Minute))
            .Select(g => new ChartDataPoint(g.Sum(x => x.Item2), g.Key.ToString("t", CultureInfo.CurrentCulture), _colorProvider.Primary))
            .ToArray();
    }

    public async Task<int> GetSellerCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .ForBasar(basarId, TransactionType.Acceptance)
            .Select(t => t.SellerId)
            .Distinct()
            .CountAsync();

    public async Task<int> GetSettledSellerCountAsync(int basarId)
        => await _db.Transactions.AsNoTracking()
            .Include(transaction => transaction.Seller)
            .ForBasar(basarId, TransactionType.Acceptance)
            .Where(transaction => transaction.Seller != null && transaction.Seller.ValueState == ValueState.Settled)
            .Select(t => t.SellerId)
            .Distinct()
            .CountAsync();

    public int GetSettlementPercentage(int sellerCount, int settledSellerCount)
    {
        if (sellerCount == 0)
        {
            return 0;
        }

        return (int)((double)settledSellerCount / sellerCount * 100.0);
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
