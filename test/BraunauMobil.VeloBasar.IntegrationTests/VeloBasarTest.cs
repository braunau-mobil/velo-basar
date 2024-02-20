using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public class VeloBasarTest
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly TestContext _context;

    public VeloBasarTest(CustomWebApplicationFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        IOptions<PrintSettings> printSettingsOption = factory.Services.GetRequiredService<IOptions<PrintSettings>>();

        _context = new TestContext(factory.Services, factory.CreateClient(), printSettingsOption.Value);
    }

    [Fact]
    public async Task Run()
    {
        await new Steps.InitialSetup(_context).Run();
        await Login();
        await new Steps.FirstBasar.Creation(_context).Run();
        await new Steps.FirstBasar.AcceptSellers(_context).Run();
        await new Steps.FirstBasar.SellProducts(_context).Run();
        await new Steps.FirstBasar.SettleSellers(_context).Run();
    }

    private async Task Login()
    {
        IHtmlDocument loginDocument = await _context.HttpClient.GetDocumentAsync("");
        loginDocument.Title.Should().Be("Log in - Velo Basar");

        IHtmlFormElement form = loginDocument.QueryForm();
        IHtmlButtonElement submitButton = loginDocument.QueryElement<IHtmlButtonElement>("button");

        IHtmlDocument postDocument = await _context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "Email", "admin@valar.me" },
            { "Password", "root" }
        });
        postDocument.Title.Should().Be("Bazaars - Velo Basar");
    }
}
