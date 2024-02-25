namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Accept;

public class Seller4(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 4 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "FirstName", "Langhöhlen" },
            { "LastName", "Siriondil" },
            { "CountryId", ID.Countries.Germany },
            { "ZIP", "2178" },
            { "City", "Nargothrond" },
            { "Street", "Börseplatz 11" },
            { "PhoneNumber", "74460686" },
            { "EMail", "langhoehlen@siriondil.me" },
            { "HasNewsletterPermission", false },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.WomansCityBike },
            { "Brand", "Epple" },
            { "Color", "white" },
            { "FrameNumber", "G#%$BIBM#$)" },
            { "Description", "No tires" },
            { "TireSize", "22" },
            { "Price", 69.54m }
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #3 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #3",
            "Thal, 6/4/2064",
            "Langhöhlen Siriondil".Line("Börseplatz 11").Line("2178 Nargothrond").Line(),
            "Seller.-ID: 4",
            "statusLink=4C6145369",
            "4C6145369",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "1 Product",
            "$69.54",
            [
                new ProductTableRowDocumentModel("7", "Epple - Woman's city bike".Line("No tires").Line(" white G#%$BIBM#$)"), "22", "$69.54", null)
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
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.LanghöhlenSiriondil, expectedDetails);
    }
}
