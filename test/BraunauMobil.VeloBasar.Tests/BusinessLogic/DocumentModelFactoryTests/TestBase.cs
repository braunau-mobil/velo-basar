using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class TestBase
{
    public TestBase()
    {
        Settings = Fixture.Create<PrintSettings>();

        Sut = new DocumentModelFactory(X.StringLocalizer, Options.Create(Settings), X.FormatProvider);
    }

    protected VeloFixture Fixture { get; } = new();

    protected PrintSettings Settings { get; }

    protected DocumentModelFactory Sut { get; }
}
