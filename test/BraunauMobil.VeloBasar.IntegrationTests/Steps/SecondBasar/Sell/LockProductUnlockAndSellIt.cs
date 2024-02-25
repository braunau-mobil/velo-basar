namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Sell;

public class LockProductUnlockAndSellIt(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument detailsDocument = await GetDetailsDocument();
        detailsDocument = await Lock(detailsDocument);
        await Unlock(detailsDocument);
        
        await Sell();
    }

    private async Task<IHtmlDocument> GetDetailsDocument()
    {
        IHtmlDocument productListDocument = await context.HttpClient.NavigateMenuAsync("Product");
        IHtmlAnchorElement page2Anchor = productListDocument.QueryElement<IHtmlAnchorElement>("a.page-link", button => button.InnerHtml == "2");
        productListDocument = await context.HttpClient.GetDocumentAsync(page2Anchor.Href);

        IHtmlAnchorElement detailsAnchor = productListDocument.QueryTableDetailsLink(10);
        IHtmlDocument detailsDocument  = await context.HttpClient.GetDocumentAsync(detailsAnchor.Href);
        detailsDocument.Title.Should().Be("Product #10 - Velo Basar");
        return detailsDocument;
    }

    private async Task<IHtmlDocument> Lock(IHtmlDocument detailsDocument)
    {
        IHtmlAnchorElement lockAnchor = detailsDocument.QueryAnchorByText("Lock");
        IHtmlDocument lockDocument = await context.HttpClient.GetDocumentAsync(lockAnchor.Href);

        IHtmlFormElement form = lockDocument.QueryForm();
        IHtmlButtonElement saveButton = lockDocument.QueryButtonByText("Save");
        detailsDocument = await context.HttpClient.SendFormAsync(form, saveButton, new Dictionary<string, object>
        {
            { "Notes", "Locked!" }
        });
        detailsDocument.Title.Should().Be("Product #10 - Velo Basar");
        return detailsDocument;
    }

    private async Task Unlock(IHtmlDocument detailsDocument)
    {
        IHtmlAnchorElement unlockAnchor = detailsDocument.QueryAnchorByText("Unlock");
        IHtmlDocument unlockDocument = await context.HttpClient.GetDocumentAsync(unlockAnchor.Href);

        IHtmlFormElement form = unlockDocument.QueryForm();
        IHtmlButtonElement saveButton = unlockDocument.QueryButtonByText("Save");
        detailsDocument = await context.HttpClient.SendFormAsync(form, saveButton, new Dictionary<string, object>
        {
            { "Notes", "Un-Locked!" }
        });
        detailsDocument.Title.Should().Be("Product #10 - Velo Basar");
    }

    private async Task Sell()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");

        cartDocument = await context.AddProductToCart(cartDocument, 10);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);

        successDocument.Title.Should().Be("Sale #2 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #2",
            "Thal, 6/4/2064",
            "1 Product",
            "$106.75",
            [
                new ProductTableRowDocumentModel("10", "Seidel & Naumann - Unicycle".Line("SALIKO_52513").Line(" lavender"), "14", "$106.75", "* Frór Bilbo, Helmut-Zilk-Platz 27, 8475 Andúnië"),
            ])
        );
    }
}
