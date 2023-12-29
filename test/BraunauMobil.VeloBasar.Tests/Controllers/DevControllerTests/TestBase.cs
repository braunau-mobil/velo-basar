using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new DevController(AppContext, DataGeneratorService, new UserManagerMock(), Router);
    }

    protected IAppContext AppContext { get; } = X.StrictFake<IAppContext>();

    protected IDataGeneratorService DataGeneratorService { get; } = X.StrictFake<IDataGeneratorService>();

    protected Fixture Fixture { get; } = new ();

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected DevController Sut { get; }

}
