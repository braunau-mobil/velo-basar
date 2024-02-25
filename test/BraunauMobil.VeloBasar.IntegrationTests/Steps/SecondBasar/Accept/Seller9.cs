namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Accept;

public class Seller9(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 9 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "BankAccountHolder", "Mallor M.F. Fimbrethil" },
            { "City", "Edhellond" },
            { "CountryId", ID.Countries.Austria },
            { "EMail", "mallor@fimbrethil.me" },
            { "FirstName", "Mallor" },
            { "HasNewsletterPermission", true },
            { "IBAN", "" },
            { "LastName", "Fimbrethil" },
            { "Street", "Schönlaterngasse 16" },
            { "PhoneNumber", "22870292" },
            { "ZIP", "2758" },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.RoadBike },
            { "Brand", "Idworx" },
            { "Color", "orange" },
            { "FrameNumber", "12a2dc85-" },
            { "Description", "IDWORX_768016" },
            { "TireSize", "24" },
            { "Price", 8.49M },
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #9 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #9",
            "Thal, 6/4/2064",
            "Mallor Fimbrethil".Line("Schönlaterngasse 16").Line("2758 Edhellond").Line(),
            "Seller.-ID: 9",
            "statusLink=4D6194669",
            "4D6194669",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "1 Product",
            "$8.49",
            [
                new ProductTableRowDocumentModel("16", "Idworx - Road bike".Line("IDWORX_768016").Line(" orange 12a2dc85-"), "24", "$8.49", null),
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
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.MallorFimbrethil, expectedDetails);
    }
}
