namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class AcceptSellers(TestContext context)
{
    public async Task Run()
    {
        await Seller2();
        await AssertOverview();
    }

    private async Task Seller2()
    {
        const string expectedTitle = "Acceptance for seller with ID: 2 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "FirstName", "Meneldor" },
            { "LastName", "Borondir" },
            { "CountryId", ID.Countries.Austria },
            { "ZIP", "2828" },
            { "City", "Bree" },
            { "Street", "Heßgasse 10" },
            { "PhoneNumber", "9332429156" },
            { "EMail", "meneldor@borondir.me" },
            { "HasNewsletterPermission", true },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.Scooter },
            { "Brand", "Additive" },
            { "Color", "blue" },
            { "FrameNumber", "X290jbgn" },
            { "Description", "X45" },
            { "TireSize", "16" },
            { "Price", 51.06m }
        });
        enterProductsDocument = await EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.Scooter },
            { "Brand", "Toxy" },
            { "Color", "white" },
            { "Description", "TY 66-17" },
            { "TireSize", "17" },
            { "Price", 45.75 }
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");
        
        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #1 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #1",
            "Thal, 6/4/2064",
            "Meneldor Borondir".Line("Heßgasse 10").Line("2828 Bree").Line(),
            "Seller.-ID: 2",
            "statusLink=4D652426F",
            "4D652426F",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "2 Product",
            "$96.81",
            [
                new ProductTableRowDocumentModel("3", "Additive - Scooter".Line("X45").Line(" blue X290jbgn"), "16", "$51.06", null),
                new ProductTableRowDocumentModel("4", "Toxy - Scooter".Line("TY 66-17").Line(" white"), "17", "$45.75", null),
            ])
        );
    }

    private async Task AssertOverview()
    {
        BasarSettlementStatus basarSettlementStatus = new(false,
            new SellerGroupSettlementStatus(1, 0),
            new SellerGroupSettlementStatus(1, 0),
            new SellerGroupSettlementStatus(0, 0)
        );
        BasarDetailsModel expectedDetails = new(new BasarEntity(), basarSettlementStatus)
        {
            AcceptanceCount = 1,
            AcceptedProductsAmount = 96.81M,
            AcceptedProductsCount = 2,
            AcceptedProductTypesByAmount = [
                new ChartDataPoint(96.81M, "Scooter", Color.FromArgb(0xFF, 0x00, 0x7B, 0xFF))
            ],
            AcceptedProductTypesByCount = [
                new ChartDataPoint(2M, "Scooter", Color.FromArgb(0xFF, 0x00, 0x7B, 0xFF))
            ],
            LockedProductsCount = 0,
            LostProductsCount = 0,
            PriceDistribution = [
                new ChartDataPoint(1, "$50.00", Color.FromArgb(0xFF, 0x00, 0x7B, 0xFF))
            ],
            SaleCount = 0,
            SaleDistribution = [],
            SoldProductsAmount = 0,
            SoldProductsCount = 0,
            SoldProductTypesByCount = [],
            SoldProductTypesByAmount = [],
        };

        await context.AssertBasarDetails(2, expectedDetails);
    }

    private async Task<IHtmlDocument> EnterProduct(IHtmlDocument enterProductsDocument, string expectedTitle, IDictionary<string, object> values)
    {
        IHtmlFormElement form = enterProductsDocument.QueryForm();
        IHtmlButtonElement submitButton = enterProductsDocument.QueryButtonByText("Add");

        IHtmlDocument postDocument = await context.HttpClient.SendFormAsync(form, submitButton, values);
        postDocument.Title.Should().Be(expectedTitle);
        return postDocument;
    }
}
