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

        X.FirstContactDay = X.FirstContactDay.AddYears(1).AddMonths(1).AddDays(1).AddHours(1).AddMinutes(1).AddSeconds(1);

        await new Steps.SecondBasar.Creation(_context).Run();
        await new Steps.SecondBasar.AcceptSellers(_context).Run();
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
