namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar;

public class Creation(TestContext context)
{
    public async Task Run()
    {
        await CreateBasar();
        await AssertBasarList();
    }

    private async Task CreateBasar()
    {
        IHtmlDocument basarListDocument = await context.HttpClient.NavigateMenuAsync("Bazaars");
        IHtmlAnchorElement createBasarLink = basarListDocument.QueryAnchorByText("Create Bazaar");

        IHtmlDocument createBasarDocument = await context.HttpClient.GetDocumentAsync(createBasarLink.Href);
        createBasarDocument.Title.Should().Be("Create Bazaar - Velo Basar");

        IHtmlFormElement form = createBasarDocument.QueryForm();
        IHtmlButtonElement submitButton = createBasarDocument.QueryElement<IHtmlButtonElement>("button");

        IHtmlDocument postDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "Date", "2064-05-04" },
            { "Name", "Second Bazaar" },
            { "Location", "Thal" },
            { "ProductCommissionPercentage", 10M },
            { "State", "Enabled" },
        });
        postDocument.Title.Should().Be("Bazaars - Velo Basar");
    }

    private async Task AssertBasarList()
    {
        IHtmlDocument basarListDocument = await context.HttpClient.NavigateMenuAsync("Bazaars");
        IHtmlFormElement form = basarListDocument.QueryForm();

        basarListDocument = await context.HttpClient.SendFormAsync(form, new Dictionary<string, object>
        {
            { "State", "" }
        });

        IHtmlTableElement basarTable = basarListDocument.QueryTable();
        basarTable.Should().BeEquivalentTo(
            ["Id", "Date", "Name", "Location", "Created at", "Updated at", "State", "", "", ""],
            ["1", "Friday, May 4, 2063", "First Bazaar", "Edoras", "1/1/0001 12:00 AM", "4/5/2063 11:22 AM", "Disabled", "Details", "Edit", "Enable"],
            ["2", "Sunday, May 4, 2064", "Second Bazaar", "Thal", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Details", "Edit", "Delete"]
        );
    }
}
