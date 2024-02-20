using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class SellProducts(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");
        cartDocument.Title.Should().Be("New Sale - Velo Basar");

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement submitButton = cartDocument.QueryButtonByText("Add");

        cartDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "ProductId", ID.Products.FirstBasar.SchattenfellMagsame.Votec }
        });
        cartDocument.Title.Should().Be("New Sale - Velo Basar");

        form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);
        successDocument.Title.Should().Be("Sale #1 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - First Bazaar : Sale receipt #1",
            "Edoras, 5/4/2063",
            "1 Product",
            "$92.99",
            [
                new ProductTableRowDocumentModel("1", "Votec - Children's bike".Line("Votec VRC Comp").Line(" green 1067425379"), "24", "$92.99", "* Schattenfell Magsame, Heidenschuss 41, 6295 Hobbingen")
            ])
        );
    }
}
