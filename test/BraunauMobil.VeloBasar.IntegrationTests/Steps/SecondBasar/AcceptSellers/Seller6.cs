namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class Seller6(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 6 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "BankAccountHolder", "Chica C.C. Ciryatur" },
            { "City", "Avallóne" },
            { "CountryId", ID.Countries.Austria },
            { "EMail", "chica@ciryatur.me" },
            { "FirstName", "Chica" },
            { "HasNewsletterPermission", true },
            { "IBAN", "AT381400016314544716" },
            { "LastName", "Ciryatur" },
            { "Street", "Tiefer Graben 6" },
            { "PhoneNumber", "26005835" },
            { "ZIP", "7332" },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.ChildrensBike },
            { "Brand", "Indienrad" },
            { "Color", "brown" },
            { "Description", "INDIENRAD_246011" },
            { "TireSize", "26" },
            { "Price", 143.64M },
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #5 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #5",
            "Thal, 6/4/2064",
            "Chica Ciryatur".Line("Tiefer Graben 6").Line("7332 Avallóne").Line(),
            "Seller.-ID: 6",
            "statusLink=436864369",
            "436864369",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "1 Product",
            "$143.64",
            [
                new ProductTableRowDocumentModel("12", "Indienrad - Children's bike".Line("INDIENRAD_246011").Line(" brown"), "26", "$143.64", null),
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
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.ChicaCiryatur, expectedDetails);
    }
}
