using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new(Router, Localizer);
    }

    public IStringLocalizer<SharedResources> Localizer { get; } = X.StrictFake<IStringLocalizer<SharedResources>>();

    public IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    public DefaultVeloHtmlFactory Sut { get; }
}
