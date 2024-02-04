using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new(Router, X.StringLocalizer);
    }

    public IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    public DefaultVeloHtmlFactory Sut { get; }
}
