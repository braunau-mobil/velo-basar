namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class ProductTable(TestContext context)
{
    public async Task Run()
    {
        await CheckAll();
        await Search();
    }

    private async Task CheckAll()
    {
        IHtmlDocument document = await context.HttpClient.NavigateMenuAsync("Product");
        document.Title.Should().Be("Product - Velo Basar");

        IHtmlAnchorElement pageSizeAllAnchor = document.QueryAnchorByText("All");

        document = await context.HttpClient.GetDocumentAsync(pageSizeAllAnchor.Href);
        IHtmlTableElement table = document.QueryTable();
        table.Should().BeEquivalentTo(
            ["Id", "Brand & Type", "Description", "Price", "", "Seller", ""],
            ["3", "AdditiveScooter", "blue - 16X290jbgn - X45", "$51.06", "Settled", "2", "Details"],
            ["4", "ToxyScooter", "white - 17TY 66-17", "$45.75", "Picked up", "2", "Details"],
            ["5", "DraisinE-bike", "white - 23DR-F5", "$69.54", "Settled", "2", "Details"],
            ["6", "CyclecraftRoad bike", "yellow - 28No lights, brakes are fine", "$117.48", "Picked up", "2", "Details"],
            ["7", "EppleWoman's city bike", "white - 22G#%$BIBM#$) - No tires", "$69.54", "Settled", "4", "Details"],
            ["8", "PedalpowerSteel steed", "gray - 1695f7-4ba0-94b6-6c45a1cd0913 - VOSS SPEZIALRAD_465693", "$151.34", "Picked up", "5", "Details"],
            ["9", "Egon RaheMen's city bike", "maroon - 20VELOMOBILES_92370", "$183.53", "Picked up", "5", "Details"],
            ["10", "Seidel & NaumannUnicycle", "lavender - 14SALIKO_52513", "$106.75", "Settled", "5", "Details"],
            ["11", "LeibaWoman's city bike", "maroon - 171b26-4d44-94fe-027810ef43e7 - MIELE_398047", "$114.36", "Settled", "5", "Details"],
            ["12", "IndienradChildren's bike", "brown - 26INDIENRAD_246011", "$143.64", "Settled", "6", "Details"],
            ["13", "Subtil BikesUnicycle", "brown - 24d3b90198 - SUBTIL BIKES_963431", "$82.79", "Picked up", "4", "Details"],
            ["14", "UnivegaMen's city bike", "slate - 263eb2377a- - UNIVEGA_749336", "$149.87", "Picked up", "7", "Details"],
            ["15", "NishikiScooter", "peach - 14NISHIKI_266634", "$75.29", "Picked up", "8", "Details"],
            ["16", "IdworxRoad bike", "orange - 2412a2dc85- - IDWORX_768016", "$8.49", "Picked up", "9", "Details"]
        );
    }

    private async Task Search()
    {
        IHtmlDocument document = await context.HttpClient.NavigateMenuAsync("Product");
        document.Title.Should().Be("Product - Velo Basar");

        IHtmlFormElement form = document.QueryForm();
        IHtmlButtonElement searchButton = form.QueryButtonByText("Search");
        document = await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "SearchString", "white" }
        });

        IHtmlTableElement table = document.QueryTable();
        table.Should().BeEquivalentTo(
            ["Id", "Brand & Type", "Description", "Price", "", "Seller", ""],
            ["4", "ToxyScooter", "white - 17TY 66-17", "$45.75", "Picked up", "2", "Details"],
            ["5", "DraisinE-bike", "white - 23DR-F5", "$69.54", "Settled", "2", "Details"],
            ["7", "EppleWoman's city bike", "white - 22G#%$BIBM#$) - No tires", "$69.54", "Settled", "4", "Details"]
        );
    }
}
