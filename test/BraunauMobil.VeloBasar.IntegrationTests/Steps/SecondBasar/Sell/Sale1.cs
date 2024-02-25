namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Sell;

public class Sale1(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");
        
        cartDocument = await context.AddProductToCart(cartDocument, 5);
        cartDocument = await context.AddProductToCart(cartDocument, 7);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);

        successDocument.Title.Should().Be("Sale #1 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - Second Bazaar : Sale receipt #1",
            "Thal, 6/4/2064",
            "2 Product",
            "$139.08",
            [
                new ProductTableRowDocumentModel("5", "Draisin - E-bike".Line("DR-F5").Line(" white"), "23", "$69.54", "* Meneldor Borondir, Heßgasse 10, 2828 Bree"),
                new ProductTableRowDocumentModel("7", "Epple - Woman's city bike".Line("No tires").Line(" white G#%$BIBM#$)"), "22", "$69.54", "* Langhöhlen Siriondil, Börseplatz 11, 2178 Nargothrond"),
            ])
        );
    }
}
