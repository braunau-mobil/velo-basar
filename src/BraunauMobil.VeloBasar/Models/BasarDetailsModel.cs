namespace BraunauMobil.VeloBasar.Models;

public sealed class BasarDetailsModel
{
    public BasarDetailsModel(BasarEntity entity)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
    }

    public BasarEntity Entity { get; init; }

    public int AcceptanceCount { get; init; }

    public decimal AcceptedProductsAmount { get; init; }

    public int AcceptedProductsCount { get; init; }

    public IReadOnlyList<ChartDataPoint> AcceptedProductTypesByAmount { get; init; } = Array.Empty<ChartDataPoint>();

    public IReadOnlyList<ChartDataPoint> AcceptedProductTypesByCount { get; init; } = Array.Empty<ChartDataPoint>();

    public int LockedProductsCount { get; init; }

    public int LostProductsCount { get; init; }

    public IReadOnlyList<ChartDataPoint> PriceDistribution { get; init; } = Array.Empty<ChartDataPoint>();

    public int SaleCount { get; init; }

    public IReadOnlyList<ChartDataPoint> SaleDistribution { get; init; } = Array.Empty<ChartDataPoint>();

    public int SellerCount { get; init; }

    public int SettlementPercentage { get; init; }

    public decimal SoldProductsAmount { get; init; }

    public int SoldProductsCount { get; init; }

    public IReadOnlyList<ChartDataPoint> SoldProductTypesByCount { get; init; } = Array.Empty<ChartDataPoint>();

    public IReadOnlyList<ChartDataPoint> SoldProductTypesByAmount { get; init; } = Array.Empty<ChartDataPoint>();   
}
