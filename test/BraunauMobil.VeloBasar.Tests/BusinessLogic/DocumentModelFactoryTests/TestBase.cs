using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class TestBase
{
    public TestBase()
    {
        Settings = Fixture.Create<PrintSettings>();

        Sut = new DocumentModelFactory(new StringLocalizerMock<SharedResources>(), Options.Create(Settings));
    }

    protected VeloFixture Fixture { get; } = new();

    protected PrintSettings Settings { get; }

    protected DocumentModelFactory Sut { get; }
}
