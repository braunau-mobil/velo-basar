namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class Seller7(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 7 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "BankAccountHolder", "Folcwine F.G. Gollum" },
            { "City", "Unterharg" },
            { "CountryId", ID.Countries.Germany },
            { "EMail", "folcwine@gollum.me" },
            { "FirstName", "Folcwine" },
            { "HasNewsletterPermission", true },
            { "IBAN", "" },
            { "LastName", "Gollum" },
            { "Street", "Domgasse 25" },
            { "PhoneNumber", "192930419" },
            { "ZIP", "7356" },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.MansCityBike },
            { "Brand", "Univega" },
            { "Color", "slate" },
            { "FrameNumber", "3eb2377a-" },
            { "Description", "UNIVEGA_749336" },
            { "TireSize", "26" },
            { "Price", 149.87M },
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #6 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #6",
            "Thal, 6/4/2064",
            "Folcwine Gollum".Line("Domgasse 25").Line("7356 Unterharg").Line(),
            "Seller.-ID: 7",
            "statusLink=466F7476F",
            "466F7476F",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "1 Product",
            "$149.87",
            [
                new ProductTableRowDocumentModel("13", "Univega - Men's city bike".Line("UNIVEGA_749336").Line(" slate 3eb2377a-"), "26", "$149.87", null),
            ])
        );

        SellerDetailsModel expectedDetails = new (new SellerEntity())
        {
            AcceptedProductCount = 1,
            NotSoldProductCount = 1,
            PickedUpProductCount = 0,
            SettlementAmout = 0,
            SoldProductCount = 0
        };
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.FolcwineGollum, expectedDetails);
    }
}
