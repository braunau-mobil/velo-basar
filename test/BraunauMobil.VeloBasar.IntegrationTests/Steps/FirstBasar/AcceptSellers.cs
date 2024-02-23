namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class AcceptSellers(TestContext context)
{
    public async Task Run()
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
        enterProductsDocument.Title.Should().Be("Acceptance for seller with ID: 1 - Enter products - Velo Basar");

        enterProductsDocument = await EnterProduct1(enterProductsDocument);
        enterProductsDocument = await EnterProduct2(enterProductsDocument);

        IHtmlAnchorElement saveAnchor = enterProductsDocument.QueryAnchorByText("Save accept session");
        
        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(saveAnchor.Href);
        successDocument.Title.Should().Be("Acceptance #1 - Velo Basar");

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

    private async Task<IHtmlDocument> EnterProduct1(IHtmlDocument enterProductsDocument)
    {
        IHtmlFormElement form = enterProductsDocument.QueryForm();
        IHtmlButtonElement submitButton = enterProductsDocument.QueryButtonByText("Add");

        IHtmlDocument postDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.ChildrensBike },
            { "Brand", "Votec" },
            { "Color", "green" },
            { "FrameNumber", "1067425379" },
            { "Description", "Votec VRC Comp" },
            { "TireSize", "24" },
            { "Price", 92.99m }
        });
        postDocument.Title.Should().Be("Acceptance for seller with ID: 1 - Enter products - Velo Basar");
        return postDocument;
    }

    private async Task<IHtmlDocument> EnterProduct2(IHtmlDocument enterProductsDocument)
    {
        IHtmlFormElement form = enterProductsDocument.QueryForm();
        IHtmlButtonElement submitButton = enterProductsDocument.QueryButtonByText("Add");

        IHtmlDocument postDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "TypeId", ID.ProductTypes.SteelSteed },
            { "Brand", "KTM" },
            { "Color", "red" },
            { "FrameNumber", "1239209209" },
            { "Description", "Steed 1" },
            { "TireSize", "22" },
            { "Price", 98.89m }
        });
        postDocument.Title.Should().Be("Acceptance for seller with ID: 1 - Enter products - Velo Basar");
        return postDocument;
    }
}
