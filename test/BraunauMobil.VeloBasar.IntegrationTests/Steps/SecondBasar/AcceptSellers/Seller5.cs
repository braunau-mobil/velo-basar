namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class Seller5(TestContext context)
{
    public async Task Run()
    {
        const string expectedTitle = "Acceptance for seller with ID: 5 - Enter products - Velo Basar";

        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement submitButton = newAcceptanceDocument.QueryButtonByText("Continue");

        IHtmlDocument enterProductsDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "BankAccountHolder", "Frór F.B. Bilbo" },
            { "City", "Andúnië" },
            { "CountryId", ID.Countries.Germany },
            { "EMail", "fror@bilbo.me" },
            { "FirstName", "Frór" },
            { "HasNewsletterPermission", false },
            { "LastName", "Bilbo" },
            { "Street", "Helmut-Zilk-Platz 27" },
            { "PhoneNumber", "14712192" },
            { "ZIP", "8475" },
        });
        enterProductsDocument.Title.Should().Be(expectedTitle);

        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.SteelSteed },
            { "Brand", "Pedalpower" },
            { "Color", "gray" },
            { "FrameNumber", "95f7-4ba0-94b6-6c45a1cd0913" },
            { "Description", "VOSS SPEZIALRAD_465693" },
            { "TireSize", "16" },
            { "Price", 151.34M },
        });
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.MansCityBike },
            { "Brand", "Egon Rahe" },
            { "Color", "maroon" },
            { "Description", "VELOMOBILES_92370" },
            { "TireSize", "20" },
            { "Price", 183.53M },
        });
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.Unicycle },
            { "Brand", "Seidel & Naumann" },
            { "Color", "lavender" },
            { "Description", "SALIKO_52513" },
            { "TireSize", "14" },
            { "Price", 106.75M },
        });
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.WomansCityBike },
            { "Brand", "Leiba" },
            { "Color", "maroon" },
            { "FrameNumber", "1b26-4d44-94fe-027810ef43e7" },
            { "Description", "MIELE_398047" },
            { "TireSize", "17" },
            { "Price", 114.36M },
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #4 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #4",
            "Thal, 6/4/2064",
            "Frór Bilbo".Line("Helmut-Zilk-Platz 27").Line("8475 Andúnië").Line(),
            "Seller.-ID: 5",
            "statusLink=467254269",
            "467254269",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "4 Product",
            "$555.98",
            [
                new ProductTableRowDocumentModel("8", "Pedalpower - Steel steed".Line("VOSS SPEZIALRAD_465693").Line(" gray 95f7-4ba0-94b6-6c45a1cd0913"), "16", "$151.34", null),
                new ProductTableRowDocumentModel("9", "Egon Rahe - Men's city bike".Line("VELOMOBILES_92370").Line(" maroon"), "20", "$183.53", null),
                new ProductTableRowDocumentModel("10", "Seidel & Naumann - Unicycle".Line("SALIKO_52513").Line(" lavender"), "14", "$106.75", null),
                new ProductTableRowDocumentModel("11", "Leiba - Woman's city bike".Line("MIELE_398047").Line(" maroon 1b26-4d44-94fe-027810ef43e7"), "17", "$114.36", null)
            ])
        );

        SellerDetailsModel expectedDetails = new (new SellerEntity())
        {
            AcceptedProductCount = 4,
            NotSoldProductCount = 4,
            PickedUpProductCount = 0,
            SettlementAmout = 0,
            SoldProductCount = 0
        };
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.FrórBilbo, expectedDetails);
    }
}
