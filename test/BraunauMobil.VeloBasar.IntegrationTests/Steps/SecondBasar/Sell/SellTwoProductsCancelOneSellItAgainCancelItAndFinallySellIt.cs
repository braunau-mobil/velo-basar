namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Sell;

public class SellTwoProductsCancelOneSellItAgainCancelItAndFinallySellIt(TestContext context)
{
    public async Task Run()
    {
        await Sell();
        await Cancel();
        await SellAgain();
        await CancelAgain();
        await SellFinally();
        await CheckProduct11Details();
        await CheckProduct12Details();
    }

    private async Task Sell()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");
        
        cartDocument = await context.AddProductToCart(cartDocument, 11);
        cartDocument = await context.AddProductToCart(cartDocument, 12);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);

        successDocument.Title.Should().Be("Sale #4 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #4",
            "Thal, 6/4/2064",
            "2 Product",
            "$258.00",
            [
                new ProductTableRowDocumentModel("11", "Leiba - Woman's city bike".Line("MIELE_398047").Line(" maroon 1b26-4d44-94fe-027810ef43e7"), "17", "$114.36", "* Frór Bilbo, Helmut-Zilk-Platz 27, 8475 Andúnië"),
                new ProductTableRowDocumentModel("12", "Indienrad - Children's bike".Line("INDIENRAD_246011").Line(" brown"), "26", "$143.64", "* Chica Ciryatur, Tiefer Graben 6, 7332 Avallóne"),
            ])
        );
    }

    private async Task Cancel()
    {
        IHtmlDocument cancelDocument = await context.HttpClient.NavigateMenuAsync("Cancellate");

        IHtmlFormElement form = cancelDocument.QueryForm();
        IHtmlButtonElement okButton = cancelDocument.QueryButtonByText("OK");
        
        IHtmlDocument selectProductsDocument = await context.HttpClient.SendFormAsync(form, okButton, new Dictionary<string, object>()
        {
            { "SaleNumber" , "4"},
        });

        form = selectProductsDocument.QueryForm();
        IHtmlButtonElement cancellateButton = selectProductsDocument.QueryButtonByText("Cancellate");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, cancellateButton, new Dictionary<string, object>
        {
            { "Products[1].IsSelected", true }
        });
        successDocument.Title.Should().Be("Cancellation #1 - Velo Basar");

        IHtmlAnchorElement saleDetailsAnchor = successDocument.QueryAnchorByText("Predecessor: Sale #4");
        IHtmlDocument saleDetailsDocument = await context.HttpClient.GetDocumentAsync(saleDetailsAnchor.Href);

        IHtmlAnchorElement voucherAnchor = saleDetailsDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #4",
            "Thal, 6/4/2064",
            "1 Product",
            "$114.36",
            [
                new ProductTableRowDocumentModel("11", "Leiba - Woman's city bike".Line("MIELE_398047").Line(" maroon 1b26-4d44-94fe-027810ef43e7"), "17", "$114.36", "* Frór Bilbo, Helmut-Zilk-Platz 27, 8475 Andúnië"),
            ])
        );
    }

    private async Task SellAgain()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");

        cartDocument = await context.AddProductToCart(cartDocument, 12);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);

        successDocument.Title.Should().Be("Sale #5 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #5",
            "Thal, 6/4/2064",
            "1 Product",
            "$143.64",
            [
                new ProductTableRowDocumentModel("12", "Indienrad - Children's bike".Line("INDIENRAD_246011").Line(" brown"), "26", "$143.64", "* Chica Ciryatur, Tiefer Graben 6, 7332 Avallóne"),
            ])
        );
    }

    private async Task CancelAgain()
    {
        IHtmlDocument cancelDocument = await context.HttpClient.NavigateMenuAsync("Cancellate");

        IHtmlFormElement form = cancelDocument.QueryForm();
        IHtmlButtonElement okButton = cancelDocument.QueryButtonByText("OK");

        IHtmlDocument selectProductsDocument = await context.HttpClient.SendFormAsync(form, okButton, new Dictionary<string, object>()
        {
            { "SaleNumber" , "5"},
        });

        form = selectProductsDocument.QueryForm();
        IHtmlButtonElement cancellateButton = selectProductsDocument.QueryButtonByText("Cancellate");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, cancellateButton, new Dictionary<string, object>
        {
            { "Products[0].IsSelected", true }
        });
        successDocument.Title.Should().Be("Cancellation #2 - Velo Basar");

        IHtmlAnchorElement saleDetailsAnchor = successDocument.QueryAnchorByText("Predecessor: Sale #5");
        IHtmlDocument saleDetailsDocument = await context.HttpClient.GetDocumentAsync(saleDetailsAnchor.Href);

        IHtmlAnchorElement voucherAnchor = saleDetailsDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #5",
            "Thal, 6/4/2064",
            "0 Product",
            "$0.00",
            [])
        );
    }

    private async Task SellFinally()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");

        cartDocument = await context.AddProductToCart(cartDocument, 12);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);

        successDocument.Title.Should().Be("Sale #6 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #6",
            "Thal, 6/4/2064",
            "1 Product",
            "$143.64",
            [
                new ProductTableRowDocumentModel("12", "Indienrad - Children's bike".Line("INDIENRAD_246011").Line(" brown"), "26", "$143.64", "* Chica Ciryatur, Tiefer Graben 6, 7332 Avallóne"),
            ])
        );
    }

    private async Task CheckProduct11Details()
    {
        IHtmlDocument detailsDocument = await GetDetailsDocument(11);
        detailsDocument.Title.Should().Be("Product #11 - Velo Basar");

        IHtmlSpanElement stateSpan = detailsDocument.QueryElement<IHtmlSpanElement>("span.badge");
        stateSpan.TextContent.Should().Be("Sold");

        IHtmlTableElement transationsTable = detailsDocument.QueryTable();
        transationsTable.Should().BeEquivalentTo(
            ["Number", "Date and Time", "Type", "Notes", "", ""],
            ["4", "5/6/2064 12:23 PM", "Acceptance", "", "Voucher", "Details"],
            ["4", "5/6/2064 12:23 PM", "Sale", "", "Voucher", "Details"]
            );
    }

    private async Task CheckProduct12Details()
    {
        IHtmlDocument detailsDocument = await GetDetailsDocument(12);
        detailsDocument.Title.Should().Be("Product #12 - Velo Basar");

        IHtmlSpanElement stateSpan = detailsDocument.QueryElement<IHtmlSpanElement>("span.badge");
        stateSpan.TextContent.Should().Be("Sold");

        IHtmlTableElement transationsTable = detailsDocument.QueryTable();
        transationsTable.Should().BeEquivalentTo(
            ["Number", "Date and Time", "Type", "Notes", "", ""],
            ["5", "5/6/2064 12:23 PM", "Acceptance", "", "Voucher", "Details"],
            ["1", "5/6/2064 12:23 PM", "Cancellation", "", "", "Details"],
            ["2", "5/6/2064 12:23 PM", "Cancellation", "", "", "Details"],
            ["6", "5/6/2064 12:23 PM", "Sale", "", "Voucher", "Details"]
            );
    }

    private async Task<IHtmlDocument> GetDetailsDocument(int productId)
    {
        IHtmlDocument productListDocument = await context.HttpClient.NavigateMenuAsync("Product");
        IHtmlFormElement form = productListDocument.QueryForm();
        IHtmlButtonElement searchButton = form.QueryButtonByText("Search");
        IHtmlDocument detailsDocument = await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "SearchString", productId }
        });
        return detailsDocument;
    }

}
