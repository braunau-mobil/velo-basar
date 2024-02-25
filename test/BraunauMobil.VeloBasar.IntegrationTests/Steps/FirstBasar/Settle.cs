namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class Settle(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument sellerList = await context.HttpClient.NavigateMenuAsync("Seller");
        sellerList.Title.Should().Be("Seller - Velo Basar");
        IHtmlAnchorElement detailsLink = sellerList.QueryTableDetailsLink(ID.Sellers.SchattenfellMagsame);

        IHtmlDocument sellerDetails = await context.HttpClient.GetDocumentAsync(detailsLink.Href);
        sellerDetails.Title.Should().Be("Seller #1 Schattenfell Magsame - Velo Basar");
        IHtmlAnchorElement settleLink = sellerDetails.QueryAnchorByText("Settle");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(settleLink.Href);
        successDocument.Title.Should().Be("Settlement #1 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SettlementDocumentModel document = await context.HttpClient.GetSettlementDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SettlementDocument(
            "XYZ - First Bazaar : Settlement #1",
            "Edoras, 5/4/2063",
            "Schattenfell Magsame".Line("Heidenschuss 41").Line("6295 Hobbingen").Line(),
            "Seller.-ID: 1",
            "Sales commission (10.00% of $92.99):",
            "$92.99",
            "$9.30",
            "$83.69",
            "1 Product",
            "$92.99",
            [
                new ProductTableRowDocumentModel("1", "Votec - Children's bike".Line("Votec VRC Comp").Line(" green 1067425379"), "24", "$92.99", null)
            ],
            "1 Product",
            "$98.89",
            [
                new ProductTableRowDocumentModel("2", "KTM - Steel steed".Line("Steed 1").Line(" red 1239209209"), "22", "$98.89", null)
            ],
            false,
            "Schattenfell Magsame",
            "",
            context.BankingQrCode("Schattenfell Magsame", "EUR83.69", "XYZ - Revenue First Bazaar"),
            "Edoras on Thursday, April 5, 2063 at 11:22 AM"
            )
        );

        await AssertBasarDetails();
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(true,
            new SellerGroupSettlementStatus(1, 1),
            new SellerGroupSettlementStatus(1, 1),
            new SellerGroupSettlementStatus(0, 0)
        );
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
            PriceDistribution = [
                new ChartDataPoint(2, "$100.00", X.AnyColor)
            ],
            SaleCount = 1,
            SaleDistribution = [
                new ChartDataPoint(92.99M, "11:22 AM", X.AnyColor),
            ],
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
