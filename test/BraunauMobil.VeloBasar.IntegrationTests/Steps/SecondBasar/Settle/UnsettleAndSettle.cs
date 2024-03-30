using BraunauMobil.VeloBasar.IntegrationTests;

namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Settle;

public class UnsettleAndSettle(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument detailsDocument = await OpenTransactionDetails();

        IHtmlAnchorElement unsettleAnchor = detailsDocument.QueryAnchorByText("Unsettle");
        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(unsettleAnchor.Href);
        successDocument.Title.Should().Be("Unsettlement #1 - Velo Basar");

        await SettleSeller(ID.Sellers.LanghöhlenSiriondil, "Seller #4 Langhöhlen Siriondil - Velo Basar", "Settlement #9 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #9",
            "Thal, 6/4/2064",
            "Langhöhlen Siriondil".Line("Börseplatz 11").Line("2178 Nargothrond").Line(),
            "Seller.-ID: 4",
            "Sales commission (10.00% of $69.54):",
            "$69.54",
            "$6.95",
            "$62.59",
            "1 Product",
            "$69.54",
            [
                new ProductTableRowDocumentModel("7", "Epple - Woman's city bike".Line("No tires").Line(" white G#%$BIBM#$)"), "22", "$69.54", null),
            ],
            "1 Product",
            "$82.79",
            [
                new ProductTableRowDocumentModel("13", "Subtil Bikes - Unicycle".Line("SUBTIL BIKES_963431").Line(" brown d3b90198"), "24", "$82.79", null),
            ],
            false,
            "Langhöhlen Siriondil",
            "",
            context.BankingQrCode("Langhöhlen Siriondil", "EUR62.59", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );        

        await AssertBasarDetails();
    }

    private async Task<IHtmlDocument> OpenTransactionDetails()
    {
        IHtmlDocument transactionListDocument = await context.HttpClient.NavigateMenuAsync("Transactions");
        IHtmlFormElement form = transactionListDocument.QueryForm();

        transactionListDocument = await context.HttpClient.SendFormAsync(form, new Dictionary<string, object>
        {
            { "TransactionType", "Settlement" }
        });
        IHtmlTableElement transactionTable = transactionListDocument.QueryTable();
        transactionTable.Should().BeEquivalentTo(
            ["Number", "Date and Time", "Number of procuts", "Notes", "Products value", "", ""],
            ["1", "5/6/2064 12:23 PM", "4", "", "$283.83", "Voucher", "Details"],
            ["2", "5/6/2064 12:23 PM", "1", "", "$75.29", "Voucher", "Details"],
            ["3", "5/6/2064 12:23 PM", "0", "", "$0.00", "Voucher", "Details"],
            ["4", "5/6/2064 12:23 PM", "2", "", "$152.33", "Voucher", "Details"],
            ["5", "5/6/2064 12:23 PM", "4", "", "$555.98", "Voucher", "Details"]
        );

        IHtmlAnchorElement detailsAnchor = transactionTable.QueryTableLinkByNumberAndText(4, "Details");
        return await context.HttpClient.GetDocumentAsync(detailsAnchor.Href);
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(true, 7, 0, 0, 0);
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

    private async Task SettleSeller(int sellerId, string expectedDetailsTitle, string expectedSuccessTitle, SettlementDocumentModel expectedDocument)
    {
        IHtmlDocument sellerDetailsDocument = await GetSellerDetails(sellerId);
        sellerDetailsDocument.Title.Should().Be(expectedDetailsTitle);

        IHtmlAnchorElement settleAnchor = sellerDetailsDocument.QueryAnchorByText("Settle");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(settleAnchor.Href);
        successDocument.Title.Should().Be(expectedSuccessTitle);

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SettlementDocumentModel document = await context.HttpClient.GetSettlementDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(expectedDocument);
    }

    private async Task<IHtmlDocument> GetSellerDetails(int sellerId)
    {
        IHtmlDocument sellerListDocument = await context.HttpClient.NavigateMenuAsync("Seller");
        IHtmlFormElement form = sellerListDocument.QueryForm();
        IHtmlButtonElement searchButton = sellerListDocument.QueryButtonByText("Search");

        return await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "SearchString", sellerId }
        });
    }
}
