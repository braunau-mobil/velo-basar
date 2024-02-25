namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class AcceptSellers(TestContext context)
{
    public async Task Run()
    {
        await AcceptSeller1();

        await AssertBasarDetails();
    }

    private async Task AcceptSeller1()
    {
        IHtmlDocument enterProductsDocument = await EnterSeller();
        IHtmlDocument successDocument = await EnterProducts(enterProductsDocument);
        successDocument.Title.Should().Be("Acceptance #1 - Velo Basar");

        await CheckVoucher(successDocument);
    }

    private async Task<IHtmlDocument> EnterSeller()
    {
        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "FirstName", "Schattenfell" },
            { "LastName", "Magsame" },
            { "CountryId", ID.Countries.Germany },
            { "ZIP", "6295" },
            { "City", "Hobbingen" },
            { "Street", "Heidenschuss 41" },
            { "PhoneNumber", "71904814" },
            { "EMail", "schattenfell@magsame.me" },
        });
        return enterProductsDocument;
    }

    private async Task<IHtmlDocument> EnterProducts(IHtmlDocument enterProductsDocument)
    {
        const string expectedTitle = "Acceptance for seller with ID: 1 - Enter products - Velo Basar";

        enterProductsDocument.Title.Should().Be(expectedTitle);
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.ChildrensBike },
            { "Brand", "Votec" },
            { "Color", "green" },
            { "FrameNumber", "1067425379" },
            { "Description", "Votec VRC Comp" },
            { "TireSize", "24" },
            { "Price", 92.99m }
        });
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.SteelSteed },
            { "Brand", "KTM" },
            { "Color", "red" },
            { "FrameNumber", "1239209209" },
            { "Description", "Steed 1" },
            { "TireSize", "22" },
            { "Price", 98.89m }
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");
        return await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
    }

    private async Task CheckVoucher(IHtmlDocument successDocument)
    {
        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - First Bazaar : Acceptance receipt #1",
            "Edoras, 5/4/2063",
            "Schattenfell Magsame".Line("Heidenschuss 41").Line("6295 Hobbingen").Line(),
            "Seller.-ID: 1",
            "statusLink=536314D61",
            "536314D61",
            "Edoras on Thursday, April 5, 2063 at 11:22 AM",
            "2 Product",
            "$191.88",
            [
                new ProductTableRowDocumentModel("1", "Votec - Children's bike".Line("Votec VRC Comp").Line(" green 1067425379"), "24", "$92.99", null),
                new ProductTableRowDocumentModel("2", "KTM - Steel steed".Line("Steed 1").Line(" red 1239209209"), "22", "$98.89", null),
            ])
        );
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(false,
            new SellerGroupSettlementStatus(1, 0),
            new SellerGroupSettlementStatus(1, 0),
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
            SaleCount = 0,
            SaleDistribution = [],
            SoldProductsAmount = 0,
            SoldProductsCount = 0,
            SoldProductTypesByCount = [],
            SoldProductTypesByAmount = [],
        };

        await context.AssertBasarDetails(ID.FirstBasar, expectedDetails);
    }
}
