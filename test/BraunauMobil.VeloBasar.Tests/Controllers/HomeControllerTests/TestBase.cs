using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Tests.Controllers.HomeControllerTests;

public class TestBase
{
    public TestBase()
    {
        A.CallTo(() => Router.Basar).Returns(BasarRouter);
        A.CallTo(() => Router.Setup).Returns(SetupRouter);

        Sut = new HomeController(AppContext, Router, A.Fake<ILogger<HomeController>>(), CurrentThemeCookie);
    }

    protected IAppContext AppContext { get; } = X.StrictFake<IAppContext>();


    protected IBasarRouter BasarRouter { get; } = X.StrictFake<IBasarRouter>();

    protected ICurrentThemeCookie CurrentThemeCookie { get; } = X.StrictFake<ICurrentThemeCookie>();

    protected VeloFixture Fixture { get; } = new ();

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected ISetupRouter SetupRouter { get; } = X.StrictFake<ISetupRouter>();

    protected HomeController Sut { get; }

}
