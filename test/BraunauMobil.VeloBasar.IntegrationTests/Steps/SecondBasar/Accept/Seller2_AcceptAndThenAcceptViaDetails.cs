namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Accept;

public class Seller2_AcceptAndThenAcceptViaDetails(TestContext context)
{
    public async Task Run()
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

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.Scooter },
            { "Brand", "Additive" },
            { "Color", "blue" },
            { "FrameNumber", "X290jbgn" },
            { "Description", "X45" },
            { "TireSize", "16" },
            { "Price", 51.06m }
        });
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
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

        IHtmlDocument sellerListDocument = await context.HttpClient.NavigateMenuAsync("Seller");
        IHtmlFormElement sellerListform = sellerListDocument.QueryForm();
        IHtmlDocument sellerDetailsDocument = await context.HttpClient.SendFormAsync(sellerListform, new Dictionary<string, object>
        {
            { "SearchString", ID.Sellers.MeneldorBorondir }
        });
        
        IHtmlAnchorElement acceptProductsAnchor = sellerDetailsDocument.QueryAnchorByText("Accept products");

        enterProductsDocument = await context.HttpClient.GetDocumentAsync(acceptProductsAnchor.Href);
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.EBike },
            { "Brand", "Draisin" },
            { "Color", "white" },
            { "Description", "DR-F5" },
            { "TireSize", "23" },
            { "Price", 69.54M }
        });
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.RoadBike },
            { "Brand", "Cyclecraft" },
            { "Color", "yellow" },
            { "Description", "No lights, brakes are fine" },
            { "TireSize", "28" },
            { "Price", 117.48M }
        });

        saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #2 - Velo Basar");

        voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #2",
            "Thal, 6/4/2064",
            "Meneldor Borondir".Line("Heßgasse 10").Line("2828 Bree").Line(),
            "Seller.-ID: 2",
            "statusLink=4D652426F",
            "4D652426F",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "2 Product",
            "$187.02",
            [
                new ProductTableRowDocumentModel("5", "Draisin - E-bike".Line("DR-F5").Line(" white"), "23", "$69.54", null),
                new ProductTableRowDocumentModel("6", "Cyclecraft - Road bike".Line("No lights, brakes are fine").Line(" yellow"), "28", "$117.48", null),
            ])
        );

        SellerDetailsModel expectedDetails = new(new SellerEntity())
        {
            AcceptedProductCount = 4,
            NotSoldProductCount = 4,
            PickedUpProductCount = 0,
            SettlementAmout = 0,
            SoldProductCount = 0
        };
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.MeneldorBorondir, expectedDetails);
    }
}
