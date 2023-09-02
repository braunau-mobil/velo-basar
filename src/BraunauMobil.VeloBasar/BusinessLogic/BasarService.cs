using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class BasarService
    : IBasarService
{
    private readonly VeloDbContext _db;
    private readonly IColorProvider _colorProvider;

    public BasarService(VeloDbContext db, IColorProvider colorProvider)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _colorProvider = colorProvider ?? throw new ArgumentNullException(nameof(colorProvider));
    }

    public async Task<string> GetBasarNameAsync(int id)
        => await _db.Basars
            .Where(b => b.Id == id)
            .Select(b => b.Name)
            .FirstAsync();

    public async Task<int?> GetActiveBasarIdAsync()
    {
        int id = await _db.Basars
            .Where(b => b.State == ObjectState.Enabled)
            .Select(b => b.Id)
            .FirstOrDefaultAsync<int>();

        if (id == 0)
        {
            return null;
        }
        return id;
    }

    public async Task<BasarDetailsModel> GetDetailsAsync(int id)
    {
        BasarEntity basar = await _db.Basars.AsNoTracking().FirstByIdAsync(id);

        int acceptanceCount = await _db.Transactions.AsNoTracking().ForBasar(id, TransactionType.Acceptance).CountAsync();
        int saleCount = await _db.Transactions.AsNoTracking().ForBasar(id, TransactionType.Sale).CountAsync();
        int sellerCount = await GetSellerCountAsync(id);
        int settledSellerCount = await GetSettledSellersCountAsync(id);
        int settlementPercentage = (int)((double)settledSellerCount / sellerCount * 100.0);

        ProductEntity[] acceptedProducts = await AcceptedProducts(id).ToArrayAsync();
        ProductEntity[] soldProducts = await SoldProducts(id).ToArrayAsync();

        return new BasarDetailsModel(basar)
        {
            AcceptanceCount = acceptanceCount,
            AcceptedProductsAmount = acceptedProducts.Sum(product => product.Price),
            AcceptedProductsCount = acceptedProducts.Length,
            AcceptedProductsByAmount = GroupByTypeAndAmount(acceptedProducts),
            AcceptedProductsByCount = GroupByTypeAndCount(acceptedProducts),
            LostProductsCount = acceptedProducts.Count(procuct => procuct.StorageState == StorageState.Lost),
            LockedProductsCount = acceptedProducts.Count(procuct => procuct.StorageState == StorageState.Locked),
            PriceDistribution = await GetPriceDistributionAsync(id),
            SaleCount = saleCount,
            SaleDistribution = await GetSaleDistributionAsync(id),
            SettlementPercentage = settlementPercentage,
            SellerCount = sellerCount,
            SoldProductsAmount = soldProducts.Sum(product => product.Price),
            SoldProductsCount = soldProducts.Length,
            SoldProductsByAmount = GroupByTypeAndAmount(soldProducts),
            SoldProductsByCount = GroupByTypeAndCount(soldProducts),
        };
    }

    private IQueryable<ProductEntity> AcceptedProducts(int basarId)
        => _db.Products
            .Include(product => product.Session)
            .Include(product => product.Type)
            .AsNoTracking()
            .Where(product => product.Session.BasarId == basarId);

    private IQueryable<ProductEntity> SoldProducts(int basarId)
        => AcceptedProducts(basarId)
            .Where(product => product.StorageState == StorageState.Sold);

    private IReadOnlyList<ChartDataPoint> GroupByTypeAndAmount(ProductEntity[] products)
    {
        var productTypeAmounts = products
            .GroupBy(product => product.Type.Name)
            .Select(group => new
            {
                Name = group.Key,
                Amount = group.Sum(product => product.Price)
            });

        return productTypeAmounts
            .Select(x => new ChartDataPoint(x.Amount, x.Name, _colorProvider[x.Name]))
            .ToArray();
    }

    private IReadOnlyList<ChartDataPoint> GroupByTypeAndCount(ProductEntity[] products)
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

    private async Task<int> GetSettledSellersCountAsync(int id)
    {
        return await _db.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.Seller)
            .ForBasar(id, TransactionType.Acceptance)
            .Where(transaction => transaction.Seller != null && transaction.Seller.ValueState == ValueState.Settled)
            .Select(t => t.SellerId)
            .Distinct()
            .CountAsync();
    }

    private async Task<int> GetSellerCountAsync(int id)
    {
        return await _db.Transactions
            .AsNoTracking()
            .ForBasar(id, TransactionType.Acceptance)
            .Select(t => t.SellerId)
            .Distinct()
            .CountAsync();
    }

    private async Task<IReadOnlyList<ChartDataPoint>> GetSaleDistributionAsync(int basarId)
    {
        var transactionAndPrices = await
            (from transaction in _db.Transactions
             join productToTransaction in _db.ProductToTransaction on transaction.Id equals productToTransaction.TransactionId
             join product in _db.Products on productToTransaction.ProductId equals product.Id
             where transaction.BasarId == basarId && transaction.Type == TransactionType.Sale && product.StorageState == StorageState.Sold
             select new
             {
                 transaction.TimeStamp,
                 product.Price
             })
            .AsNoTracking()
            .ToArrayAsync();

        return transactionAndPrices.GroupBy(x => new TimeOnly(x.TimeStamp.Hour, x.TimeStamp.Minute))
            .Select(g => new ChartDataPoint(g.Sum(x => x.Price), g.Key.ToString(CultureInfo.CurrentCulture), _colorProvider.Primary))
            .ToArray();
    }

    private async Task<IReadOnlyList<ChartDataPoint>> GetPriceDistributionAsync(int basarId)
    {
        IReadOnlyList<decimal> productPrices = await AcceptedProducts(basarId)
            .Select(product => product.Price)
            .ToArrayAsync();

        if (!productPrices.Any())
        {
            return Array.Empty<ChartDataPoint>();
        }

        decimal step = 10.0m;
        decimal maxPrice = productPrices.Max();
        decimal currentMin = 0.0m;
        decimal currentMax = step;

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
}
