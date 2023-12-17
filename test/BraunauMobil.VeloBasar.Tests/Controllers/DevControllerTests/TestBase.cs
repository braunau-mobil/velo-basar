using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new DevController(AppContext.Object, DataGeneratorService.Object, new UserManagerMock(), Router.Object);
    }

    public void VerifyNoOtherCalls()
    {
        AppContext.VerifyNoOtherCalls();
        DataGeneratorService.VerifyNoOtherCalls();
        Router.VerifyNoOtherCalls();
    }

    protected Mock<IAppContext> AppContext { get; } = new();

    protected Mock<IDataGeneratorService> DataGeneratorService { get; } = new();

    protected Fixture Fixture { get; } = new ();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected DevController Sut { get; }

}
