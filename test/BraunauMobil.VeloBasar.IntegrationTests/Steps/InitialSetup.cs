namespace BraunauMobil.VeloBasar.IntegrationTests.Steps;

public class InitialSetup(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument intialSetupDocument = await context.HttpClient.GetDocumentAsync("");
        intialSetupDocument.Title.Should().Be("Initial setup - Velo Basar");
        IHtmlFormElement form = intialSetupDocument.QueryForm();
        IHtmlButtonElement submitButton = intialSetupDocument.QueryElement<IHtmlButtonElement>("button");

        IHtmlDocument postDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "AdminUserEMail", "admin@valar.me" },
            { "GenerateCountries", true },
            { "GenerateProductTypes", true },
            { "GenerateZipCodes", true },
        });
        postDocument.Title.Should().Be("Log in - Velo Basar");
    }    
}
