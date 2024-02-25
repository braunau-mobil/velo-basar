namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Accept;

public class Seller4_AcceptViaSearch(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument enterProductsDocument = await Search();
        
        const string expectedTitle = "Acceptance for seller with ID: 4 - Enter products - Velo Basar";
        enterProductsDocument = await context.EnterProduct(enterProductsDocument, expectedTitle, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.Unicycle },
            { "Brand", "Subtil Bikes" },
            { "Color", "brown" },
            { "FrameNumber", "d3b90198" },
            { "Description", "SUBTIL BIKES_963431" },
            { "TireSize", "24" },
            { "Price", 82.79M },
        });

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #6 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        AcceptanceDocumentModel document = await context.HttpClient.GetAcceptanceDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.AcceptanceDocument("XYZ - Second Bazaar : Acceptance receipt #6",
            "Thal, 6/4/2064",
            "Langhöhlen Siriondil".Line("Börseplatz 11").Line("2178 Nargothrond").Line(),
            "Seller.-ID: 4",
            "statusLink=4C6145369",
            "4C6145369",
            "Thal on Tuesday, May 6, 2064 at 12:23 PM",
            "1 Product",
            "$82.79",
            [
                new ProductTableRowDocumentModel("13", "Subtil Bikes - Unicycle".Line("SUBTIL BIKES_963431").Line(" brown d3b90198"), "24", "$82.79", null),
            ])
        );

        SellerDetailsModel expectedDetails = new(new SellerEntity())
        {
            AcceptedProductCount = 2,
            NotSoldProductCount = 2,
            PickedUpProductCount = 0,
            SettlementAmout = 0,
            SoldProductCount = 0
        };
        await context.AssertSellerDetails(ID.SecondBasar, ID.Sellers.LanghöhlenSiriondil, expectedDetails);
    }

    private async Task<IHtmlDocument> Search()
    {
        IHtmlDocument newAcceptanceDocument = await context.HttpClient.NavigateMenuAsync("New Acceptance");
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");

        IHtmlFormElement form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement searchButton = newAcceptanceDocument.QueryButtonByText("Search");
        newAcceptanceDocument = await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "FirstName", "Langhöhlen" },
            { "LastName", "Siriondil" },
        });
        newAcceptanceDocument.Title.Should().Be("Acceptance - Enter seller - Velo Basar");
        
        IHtmlAnchorElement applyAnchor = newAcceptanceDocument.QueryTableApplyLink(ID.Sellers.LanghöhlenSiriondil);
        newAcceptanceDocument = await context.HttpClient.GetDocumentAsync(applyAnchor.Href);

        form = newAcceptanceDocument.QueryForm();
        IHtmlButtonElement contiuneButton = newAcceptanceDocument.QueryButtonByText("Continue");
        return await context.HttpClient.SendFormAsync(form, contiuneButton, []);
    }
}
