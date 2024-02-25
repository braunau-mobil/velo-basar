namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class Create(TestContext context)
{
    public async Task Run()
    {
        await CreateBasar();
        await EditBasar();
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
            { "Date", "2063-05-04" },
            { "Name", "First Bazaar" },
            { "Location", "Edoras" },
            { "ProductCommissionPercentage", "9" },
            { "State", "Enabled" },
        });
        postDocument.Title.Should().Be("Bazaars - Velo Basar");
    }

    private async Task EditBasar()
    {
        IHtmlDocument basarListDocument = await context.HttpClient.NavigateMenuAsync("Bazaars");
        IHtmlAnchorElement editBasarLink = basarListDocument.QueryAnchorByText("Edit");
        
        IHtmlDocument editBasarDocument = await context.HttpClient.GetDocumentAsync(editBasarLink.Href);
        editBasarDocument.Title.Should().Be("Edit Bazaar - Velo Basar");
        
        IHtmlFormElement form = editBasarDocument.QueryForm();
        IHtmlButtonElement submitButton = editBasarDocument.QueryElement<IHtmlButtonElement>("button");

        IHtmlDocument postDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "ProductCommissionPercentage", "10" },
        });
        postDocument.Title.Should().Be("Bazaars - Velo Basar");
    }
}
