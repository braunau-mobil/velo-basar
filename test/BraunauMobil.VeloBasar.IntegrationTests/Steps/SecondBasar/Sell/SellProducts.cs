namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Sell;

public class SellProducts(TestContext context)
{
    public async Task Run()
    {
        await new Sale1(context).Run();
        await new LockProductUnlockAndSellIt(context).Run();
        await new LooseProductUnlockAndSellIt(context).Run();
        await new SellTwoProductsCancelOneSellItAgainCancelItAndFinallySellIt(context).Run();

        await AssertBasarDetails();
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(false, 0, 7, 6, 1);
        BasarDetailsModel expectedDetails = new(new BasarEntity(), basarSettlementStatus)
        {
            AcceptanceCount = 9,
            AcceptedProductsAmount = 1369.43M,
            AcceptedProductsCount = 14,
            AcceptedProductTypesByAmount = [
                new ChartDataPoint(172.10M, "Scooter", X.AnyColor),
                new ChartDataPoint(69.54M, "E-bike", X.AnyColor),
                new ChartDataPoint(125.97M, "Road bike", X.AnyColor),
                new ChartDataPoint(151.34M, "Steel steed", X.AnyColor),
                new ChartDataPoint(333.40M, "Men's city bike", X.AnyColor),
                new ChartDataPoint(183.90M, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(189.54M, "Unicycle", X.AnyColor),
                new ChartDataPoint(143.64M, "Children's bike", X.AnyColor),
            ],
            AcceptedProductTypesByCount = [
                new ChartDataPoint(3, "Scooter", X.AnyColor),
                new ChartDataPoint(1, "E-bike", X.AnyColor),
                new ChartDataPoint(2, "Road bike", X.AnyColor),
                new ChartDataPoint(1, "Steel steed", X.AnyColor),
                new ChartDataPoint(2, "Men's city bike", X.AnyColor),
                new ChartDataPoint(2, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(2, "Unicycle", X.AnyColor),
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
            LockedProductsCount = 0,
            LostProductsCount = 0,
            PriceDistribution = context.PriceDistribtion(1, 1, 5, 7, 0),
            SaleCount = 6,
            SaleDistribution = [
                new ChartDataPoint(554.89M, "12:23 PM", X.AnyColor),
            ],
            SellerCount = 7,
            SoldProductsAmount = 554.89M,
            SoldProductsCount = 6,
            SoldProductTypesByAmount = [
                new ChartDataPoint(51.06M, "Scooter", X.AnyColor),
                new ChartDataPoint(69.54M, "E-bike", X.AnyColor),
                new ChartDataPoint(183.90M, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(106.75M, "Unicycle", X.AnyColor),
                new ChartDataPoint(143.64M, "Children's bike", X.AnyColor),
            ],
            SoldProductTypesByCount = [
                new ChartDataPoint(1, "Scooter", X.AnyColor),
                new ChartDataPoint(1, "E-bike", X.AnyColor),
                new ChartDataPoint(2, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(1, "Unicycle", X.AnyColor),
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
        };

        await context.AssertBasarDetails(ID.SecondBasar, expectedDetails);
    }
}
