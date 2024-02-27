namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class ProductTypes(TestContext context)
{
    public async Task Run()
    {
        await All();
        await Search();
        await Edit();
    }

    private async Task All()
    {
        IHtmlDocument listDocument = await context.HttpClient.NavigateMenuAsync("Product types");
        listDocument.Title.Should().Be("Product types - Velo Basar");

        IHtmlAnchorElement pageSizeAllAnchor = listDocument.QueryAnchorByText("All");

        listDocument = await context.HttpClient.GetDocumentAsync(pageSizeAllAnchor.Href);
        IHtmlTableElement table = listDocument.QueryTable();
        table.Should().BeEquivalentTo(
            [ "Id", "Name", "Description", "Created at", "Updated at", "State", "", "" ],
            ["5", "Children's bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Disable"],
            ["7", "E-bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["3", "Men's city bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["2", "Road bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["6", "Scooter", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["8", "Steel steed", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Disable"],
            ["1", "Unicycle", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["4", "Woman's city bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"]
        );
    }

    private async Task Search()
    {
        IHtmlDocument document = await context.HttpClient.NavigateMenuAsync("Product types");
        document.Title.Should().Be("Product types - Velo Basar");

        IHtmlFormElement form = document.QueryForm();
        IHtmlButtonElement searchButton = form.QueryButtonByText("Search");
        document = await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "SearchString", "bike" }
        });

        IHtmlTableElement table = document.QueryTable();
        table.Should().BeEquivalentTo(
            ["Id", "Name", "Description", "Created at", "Updated at", "State", "", ""],
            ["5", "Children's bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Disable"],
            ["7", "E-bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["3", "Men's city bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["2", "Road bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"],
            ["4", "Woman's city bike", "", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"]
        );
    }

    private async Task Edit()
    {
        IHtmlDocument tableDocument = await context.HttpClient.NavigateMenuAsync("Product types");
        tableDocument.Title.Should().Be("Product types - Velo Basar");

        IHtmlAnchorElement editAnchor = tableDocument.QueryTableLinkByIdAndText(ID.ProductTypes.Scooter, "Edit");
        IHtmlDocument editDocument = await context.HttpClient.GetDocumentAsync(editAnchor.Href);

        IHtmlFormElement editForm = editDocument.QueryForm();
        IHtmlButtonElement saveButton = editDocument.QueryButtonByText("Save");
        tableDocument = await context.HttpClient.SendFormAsync(editForm, saveButton, new Dictionary<string, object>
        {
            { "Description", "This is a Scooter" }
        });

        IHtmlFormElement tableForm = tableDocument.QueryForm();
        IHtmlButtonElement searchButton = tableDocument.QueryButtonByText("Search");
        tableDocument = await context.HttpClient.SendFormAsync(tableForm, searchButton, new Dictionary<string, object>
        {
            { "SearchString", ID.ProductTypes.Scooter }
        });

        IHtmlTableElement table = tableDocument.QueryTable();
        table.Should().BeEquivalentTo(
            ["Id", "Name", "Description", "Created at", "Updated at", "State", "", ""],
            ["6", "Scooter", "This is a Scooter", "4/5/2063 11:22 AM", "4/5/2063 11:22 AM", "Enabled", "Edit", "Delete"]
        );
    }
}
