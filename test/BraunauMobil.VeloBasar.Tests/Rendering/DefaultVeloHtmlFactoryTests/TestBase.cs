using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new(Router, new StringLocalizerMock<SharedResources>());
    }

    public IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    public DefaultVeloHtmlFactory Sut { get; }
}
