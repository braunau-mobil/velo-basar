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

    public IReadOnlyList<ChartDataPoint> AcceptedProductsByAmount { get; init; } = Array.Empty<ChartDataPoint>();

    public IReadOnlyList<ChartDataPoint> AcceptedProductsByCount { get; init; } = Array.Empty<ChartDataPoint>();

    public int LockedProductsCount { get; init; }

    public int LostProductsCount { get; init; }

    public IReadOnlyList<ChartDataPoint> PriceDistribution { get; init; } = Array.Empty<ChartDataPoint>();

    public int SaleCount { get; init; }

    public IReadOnlyList<ChartDataPoint> SaleDistribution { get; init; } = Array.Empty<ChartDataPoint>();

    public int SellerCount { get; init; }

    public int SettlementPercentage { get; init; }

    public decimal SoldProductsAmount { get; init; }

    public int SoldProductsCount { get; init; }

    public IReadOnlyList<ChartDataPoint> SoldProductsByCount { get; init; } = Array.Empty<ChartDataPoint>();

    public IReadOnlyList<ChartDataPoint> SoldProductsByAmount { get; init; } = Array.Empty<ChartDataPoint>();   
}
