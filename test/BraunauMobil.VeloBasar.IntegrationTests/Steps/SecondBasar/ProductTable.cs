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
            ["Id", "Brands", "Type", "Color", "Frame number", "Description", "Tire size", "Price", "", "Seller", ""],
            ["3", "Additive", "Scooter", "blue", "X290jbgn", "X45", "16", "$51.06", "Settled", "2", "Details"],
            ["4", "Toxy", "Scooter", "white", "", "TY 66-17", "17", "$45.75", "Picked up", "2", "Details"],
            ["5", "Draisin", "E-bike", "white", "", "DR-F5", "23", "$69.54", "Settled", "2", "Details"],
            ["6", "Cyclecraft", "Road bike", "yellow", "", "No lights, brakes are fine", "28", "$117.48", "Picked up", "2", "Details"],
            ["7", "Epple", "Woman's city bike", "white", "G#%$BIBM#$)", "No tires", "22", "$69.54", "Settled", "4", "Details"],
            ["8", "Pedalpower", "Steel steed", "gray", "95f7-4ba0-94b6-6c45a1cd0913", "VOSS SPEZIALRAD_465693", "16", "$151.34", "Picked up", "5", "Details"],
            ["9", "Egon Rahe", "Men's city bike", "maroon", "", "VELOMOBILES_92370", "20", "$183.53", "Picked up", "5", "Details"],
            ["10", "Seidel & Naumann", "Unicycle", "lavender", "", "SALIKO_52513", "14", "$106.75", "Settled", "5", "Details"],
            ["11", "Leiba", "Woman's city bike", "maroon", "1b26-4d44-94fe-027810ef43e7", "MIELE_398047", "17", "$114.36", "Settled", "5", "Details"],
            ["12", "Indienrad", "Children's bike", "brown", "", "INDIENRAD_246011", "26", "$143.64", "Settled", "6", "Details"],
            ["13", "Subtil Bikes", "Unicycle", "brown", "d3b90198", "SUBTIL BIKES_963431", "24", "$82.79", "Picked up", "4", "Details"],
            ["14", "Univega", "Men's city bike", "slate", "3eb2377a-", "UNIVEGA_749336", "26", "$149.87", "Picked up", "7", "Details"],
            ["15", "Nishiki", "Scooter", "peach", "", "NISHIKI_266634", "14", "$75.29", "Picked up", "8", "Details"],
            ["16", "Idworx", "Road bike", "orange", "12a2dc85-", "IDWORX_768016", "24", "$8.49", "Picked up", "9", "Details"]
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
            ["Id", "Brands", "Type", "Color", "Frame number", "Description", "Tire size", "Price", "", "Seller", ""],
            ["4", "Toxy", "Scooter", "white", "", "TY 66-17", "17", "$45.75", "Picked up", "2", "Details"],
            ["5", "Draisin", "E-bike", "white", "", "DR-F5", "23", "$69.54", "Settled", "2", "Details"],
            ["7", "Epple", "Woman's city bike", "white", "G#%$BIBM#$)", "No tires", "22", "$69.54", "Settled", "4", "Details"]
        );
    }
}
