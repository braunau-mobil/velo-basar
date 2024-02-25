namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Accept;

public class Seller8(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 8 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "BankAccountHolder", "Ansen A.G. Gróin" },
            { "City", "Tuckbergen" },
            { "CountryId", ID.Countries.Austria },
            { "EMail", "ansen@groin.me" },
            { "FirstName", "Ansen" },
            { "HasNewsletterPermission", true },
            { "IBAN", "AT062011197918821983" },
            { "LastName", "Gróin" },
            { "Street", "Liebiggasse 3" },
            { "PhoneNumber", "07935794" },
            { "ZIP", "4579" },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.Scooter },
            { "Brand", "Nishiki" },
            { "Color", "peach" },
            { "Description", "NISHIKI_266634" },
            { "TireSize", "14" },
            { "Price", 75.29M },
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #8 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #8",
            "Thal, 6/4/2064",
            "Ansen Gróin".Line("Liebiggasse 3").Line("4579 Tuckbergen").Line(),
            "Seller.-ID: 8",
            "statusLink=416E84772",
            "416E84772",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "1 Product",
            "$75.29",
            [
                new ProductTableRowDocumentModel("15", "Nishiki - Scooter".Line("NISHIKI_266634").Line(" peach"), "14", "$75.29", null),
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
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.AnsenGróin, expectedDetails);
    }
}
