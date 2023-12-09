namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IBasarStatsService
{
    Task<int> GetAcceptanceCountAsync(int basarId);
    
    decimal GetAcceptedProductsAmount(IEnumerable<ProductEntity> products);

    Task<IReadOnlyList<ProductEntity>> GetAcceptedProductsAsync(int basarId);

    int GetAcceptedProductsCount(IEnumerable<ProductEntity> products);

    IReadOnlyList<ChartDataPoint> GetAcceptedProductTypesWithAmount(IEnumerable<ProductEntity> products);

    IReadOnlyList<ChartDataPoint> GetAcceptedProductTypesWithCount(IEnumerable<ProductEntity> products);

    int GetLockedProductsCount(IEnumerable<ProductEntity> products);

    int GetLostProductsCount(IEnumerable<ProductEntity> products);

    IReadOnlyList<ChartDataPoint> GetPriceDistribution(IEnumerable<ProductEntity> products);

    Task<int> GetSaleCountAsync(int basarId);

    IReadOnlyList<ChartDataPoint> GetSaleDistribution(IEnumerable<Tuple<TimeOnly, decimal>> transactionsAndTotals);

    Task<BasarSettlementStatus> GetSettlementStatusAsync(int basarId);

    decimal GetSoldProductsAmount(IEnumerable<ProductEntity> products);

    int GetSoldProductsCount(IEnumerable<ProductEntity> products);

    Task<IReadOnlyList<Tuple<TimeOnly, decimal>>> GetSoldProductTimestampsAndPricesAsync(int basarId);       
    
    IReadOnlyList<ChartDataPoint> GetSoldProductTypesWithAmount(IEnumerable<ProductEntity> products);

    IReadOnlyList<ChartDataPoint> GetSoldProductTypesWithCount(IEnumerable<ProductEntity> products);
}
