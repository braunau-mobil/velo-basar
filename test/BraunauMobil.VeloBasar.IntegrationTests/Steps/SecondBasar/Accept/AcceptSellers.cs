namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Accept;

public class AcceptSellers(TestContext context)
{
    public async Task Run()
    {
        await new Seller2_AcceptAndThenAcceptViaDetails(context).Run();
        await new Seller3_CancelOnEnterProducts(context).Run();
        await new Seller4(context).Run();
        await new Seller5(context).Run();
        await new Seller6(context).Run();
        await new Seller4_AcceptViaSearch(context).Run();
        await new Seller7(context).Run();
        await new Seller8(context).Run();
        await new Seller9(context).Run();

        await AssertBasarDetails();
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(false, 0, 7, 7, 0);
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
            SaleCount = 0,
            SaleDistribution = [],
            SellerCount = 7,
            SoldProductsAmount = 0,
            SoldProductsCount = 0,
            SoldProductTypesByCount = [],
            SoldProductTypesByAmount = [],
        };

        await context.AssertBasarDetails(ID.SecondBasar, expectedDetails);
    }
}
