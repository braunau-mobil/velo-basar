namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Sell;

public class LooseProductUnlockAndSellIt(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument detailsDocument = await GetDetailsDocument();
        detailsDocument = await Loose(detailsDocument);
        await Unlock(detailsDocument);
        
        await Sell();
    }

    private async Task<IHtmlDocument> GetDetailsDocument()
    {
        IHtmlDocument productListDocument = await context.HttpClient.NavigateMenuAsync("Product");
        IHtmlFormElement form = productListDocument.QueryForm();
        IHtmlButtonElement searchButton = form.QueryButtonByText("Search");
        IHtmlDocument detailsDocument = await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "SearchString", 3 }
        });
        detailsDocument.Title.Should().Be("Product #3 - Velo Basar");
        return detailsDocument;
    }

    private async Task<IHtmlDocument> Loose(IHtmlDocument detailsDocument)
    {
        IHtmlAnchorElement missingAnchor = detailsDocument.QueryAnchorByText("Missing");
        IHtmlDocument setLostDocument = await context.HttpClient.GetDocumentAsync(missingAnchor.Href);

        IHtmlFormElement form = setLostDocument.QueryForm();
        IHtmlButtonElement saveButton = setLostDocument.QueryButtonByText("Save");
        detailsDocument = await context.HttpClient.SendFormAsync(form, saveButton, new Dictionary<string, object>
        {
            { "Notes", "Lost!" }
        });
        detailsDocument.Title.Should().Be("Product #3 - Velo Basar");
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
    }

    private async Task Sell()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");

        cartDocument = await context.AddProductToCart(cartDocument, 3);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);

        successDocument.Title.Should().Be("Sale #3 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #3",
            "Thal, 6/4/2064",
            "1 Product",
            "$51.06",
            [
                new ProductTableRowDocumentModel("3", "Additive - Scooter".Line("X45").Line(" blue X290jbgn"), "16", "$51.06", "* Meneldor Borondir, Heßgasse 10, 2828 Bree"),
            ])
        );
    }
}
