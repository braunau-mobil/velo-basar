using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Logging;
using Xan.AspNetCore.Mvc.Crud;

namespace BraunauMobil.VeloBasar.Tests.Controllers.HomeControllerTests;

public class TestBase
{
    public TestBase()
    {
        Router.Setup(_ => _.Basar)
            .Returns(BasarRouter.Object);
        Router.Setup(_ => _.Setup)
            .Returns(SetupRouter.Object);

        Sut = new HomeController(AppContext.Object, Router.Object, Mock.Of<ILogger<HomeController>>(), CurrentThemeCookie.Object);
    }

    public void VerifyNoOtherCalls()
    {
        AppContext.VerifyNoOtherCalls();
        BasarRouter.VerifyNoOtherCalls();
        CurrentThemeCookie.VerifyNoOtherCalls();
        SetupRouter.VerifyNoOtherCalls();
    }

    protected Mock<IAppContext> AppContext { get; } = new();


    protected Mock<ICrudRouter<BasarEntity>> BasarRouter { get; } = new();

    protected Mock<ICurrentThemeCookie> CurrentThemeCookie { get; } = new ();

    protected Fixture Fixture { get; } = new ();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected Mock<ISetupRouter> SetupRouter { get; } = new ();

    protected HomeController Sut { get; }

}
