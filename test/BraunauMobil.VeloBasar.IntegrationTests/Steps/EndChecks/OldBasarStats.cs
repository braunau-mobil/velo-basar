namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.EndChecks;

public class OldBasarStats(TestContext context)
{
    public async Task Run()
    {
        BasarSettlementStatus basarSettlementStatus = new(false, 0, 0, 0);
        BasarDetailsModel expectedDetails = new(new BasarEntity(), basarSettlementStatus)
        {
            AcceptanceCount = 1,
            AcceptedProductsAmount = 191.88M,
            AcceptedProductsCount = 2,
            AcceptedProductTypesByAmount = [
                new ChartDataPoint(92.99m, "Children's bike", X.AnyColor),
                new ChartDataPoint(98.89m, "Steel steed", X.AnyColor)
            ],
            AcceptedProductTypesByCount = [
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
                new ChartDataPoint(1, "Steel steed", X.AnyColor)
            ],
            LockedProductsCount = 0,
            LostProductsCount = 0,
            PriceDistribution = context.PriceDistribtion(0, 0, 2, 0, 0),
            SaleCount = 1,
            SaleDistribution = [
                new ChartDataPoint(92.99M, "11:22 AM", X.AnyColor),
            ],
            SellerCount = 1,
            SoldProductsAmount = 92.99m,
            SoldProductsCount = 1,
            SoldProductTypesByCount = [
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
            SoldProductTypesByAmount = [
                new ChartDataPoint(92.99m, "Children's bike", X.AnyColor),
            ],
        };

        await context.AssertBasarDetails(ID.FirstBasar, expectedDetails);
    }
}
