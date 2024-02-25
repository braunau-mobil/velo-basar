namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.AcceptSellers;

public class AcceptSellers(TestContext context)
{
    public async Task Run()
    {
        await new Seller2_AcceptAndThenAcceptViaDetails(context).Run();
        await new Seller3_CancelOnEnterProducts(context).Run();
        await new Seller4(context).Run();
        await new Seller5(context).Run();
        await new Seller6(context).Run();
        await new Seller7(context).Run();
        await new Seller8(context).Run();
        await new Seller9(context).Run();

        await AssertBasarDetails();
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(false,
            new SellerGroupSettlementStatus(7, 0),
            new SellerGroupSettlementStatus(7, 0),
            new SellerGroupSettlementStatus(0, 0)
        );
        BasarDetailsModel expectedDetails = new(new BasarEntity(), basarSettlementStatus)
        {
            AcceptanceCount = 8,
            AcceptedProductsAmount = 1286.64M,
            AcceptedProductsCount = 13,
            AcceptedProductTypesByAmount = [
                new ChartDataPoint(172.10M, "Scooter", X.AnyColor),
                new ChartDataPoint(69.54M, "E-bike", X.AnyColor),
                new ChartDataPoint(125.97M, "Road bike", X.AnyColor),
                new ChartDataPoint(151.34M, "Steel steed", X.AnyColor),
                new ChartDataPoint(333.40M, "Men's city bike", X.AnyColor),
                new ChartDataPoint(183.90M, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(106.75M, "Unicycle", X.AnyColor),
                new ChartDataPoint(143.64M, "Children's bike", X.AnyColor),
            ],
            AcceptedProductTypesByCount = [
                new ChartDataPoint(3, "Scooter", X.AnyColor),
                new ChartDataPoint(1, "E-bike", X.AnyColor),
                new ChartDataPoint(2, "Road bike", X.AnyColor),
                new ChartDataPoint(1, "Steel steed", X.AnyColor),
                new ChartDataPoint(2, "Men's city bike", X.AnyColor),
                new ChartDataPoint(2, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(1, "Unicycle", X.AnyColor),
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
            LockedProductsCount = 0,
            LostProductsCount = 0,
            PriceDistribution = [
                new ChartDataPoint(1, "$10.00", X.AnyColor),
                new ChartDataPoint(1, "$50.00", X.AnyColor),
                new ChartDataPoint(1, "$60.00", X.AnyColor),
                new ChartDataPoint(2, "$70.00", X.AnyColor),
                new ChartDataPoint(1, "$80.00", X.AnyColor),
                new ChartDataPoint(1, "$110.00", X.AnyColor),
                new ChartDataPoint(2, "$120.00", X.AnyColor),
                new ChartDataPoint(2, "$150.00", X.AnyColor),
                new ChartDataPoint(1, "$160.00", X.AnyColor),
                new ChartDataPoint(1, "$190.00", X.AnyColor),
            ],
            SaleCount = 0,
            SaleDistribution = [],
            SoldProductsAmount = 0,
            SoldProductsCount = 0,
            SoldProductTypesByCount = [],
            SoldProductTypesByAmount = [],
        };

        await context.AssertBasarDetails(ID.SecondBasar, expectedDetails);
    }
}
