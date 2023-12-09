using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class BasarService
    : IBasarService
{
    private readonly VeloDbContext _db;
    private readonly IBasarStatsService _statsService;

    public BasarService(VeloDbContext db, IBasarStatsService statsService)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _statsService = statsService ?? throw new ArgumentNullException(nameof(statsService));
    }

    public async Task<string> GetBasarNameAsync(int id)
        => await _db.Basars.AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => b.Name)
            .FirstAsync();

    public async Task<int?> GetActiveBasarIdAsync()
    {
        int id = await _db.Basars.AsNoTracking()
            .Where(b => b.State == ObjectState.Enabled)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();

        if (id == 0)
        {
            return null;
        }
        return id;
    }

    public async Task<BasarDetailsModel> GetDetailsAsync(int id)
    {
        BasarEntity basar = await _db.Basars.AsNoTracking()
            .FirstByIdAsync(id);

        BasarSettlementStatus settlementStatus = await _statsService.GetSettlementStatusAsync(id);

        IReadOnlyList<ProductEntity> acceptedProducts = await _statsService.GetAcceptedProductsAsync(id);
        IReadOnlyList<Tuple<TimeOnly, decimal>> soldProductTimestampsAndPrices = await _statsService.GetSoldProductTimestampsAndPricesAsync(id);

        return new BasarDetailsModel(basar, settlementStatus)
        {
            AcceptanceCount = await _statsService.GetAcceptanceCountAsync(id),
            AcceptedProductsAmount = _statsService.GetAcceptedProductsAmount(acceptedProducts),
            AcceptedProductsCount = _statsService.GetAcceptedProductsCount(acceptedProducts),
            AcceptedProductTypesByAmount = _statsService.GetAcceptedProductTypesWithAmount(acceptedProducts),
            AcceptedProductTypesByCount = _statsService.GetAcceptedProductTypesWithCount(acceptedProducts),
            LostProductsCount = _statsService.GetLostProductsCount(acceptedProducts),
            LockedProductsCount = _statsService.GetLockedProductsCount(acceptedProducts),
            PriceDistribution = _statsService.GetPriceDistribution(acceptedProducts),
            SaleCount = await _statsService.GetSaleCountAsync(id),
            SaleDistribution = _statsService.GetSaleDistribution(soldProductTimestampsAndPrices),
            SoldProductsAmount = _statsService.GetSoldProductsAmount(acceptedProducts),
            SoldProductsCount = _statsService.GetSoldProductsCount(acceptedProducts),
            SoldProductTypesByAmount = _statsService.GetSoldProductTypesWithAmount(acceptedProducts),
            SoldProductTypesByCount = _statsService.GetSoldProductTypesWithCount(acceptedProducts),
        };
    }
}
