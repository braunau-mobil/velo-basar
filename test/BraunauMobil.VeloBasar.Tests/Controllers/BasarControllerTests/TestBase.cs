using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Xan.AspNetCore.Mvc.Crud;

namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new BasarController(BasarService.Object, Router.Object);

        Router.Setup(_ => _.Basar)
            .Returns(BasarRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        BasarService.VerifyNoOtherCalls();
        BasarRouter.VerifyNoOtherCalls();
    }

    protected Mock<ICrudRouter<BasarEntity>> BasarRouter { get; } = new();

    protected Mock<IBasarService> BasarService { get; } = new ();

    protected Fixture Fixture { get; } = new ();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected BasarController Sut { get; }
}
