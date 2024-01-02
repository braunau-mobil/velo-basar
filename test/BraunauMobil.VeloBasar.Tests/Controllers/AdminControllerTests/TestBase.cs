using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new AdminController(AdminService, Clock);
    }

    protected IAdminService AdminService { get; } = X.StrictFake<IAdminService>();

    protected ClockMock Clock { get; } = new();

    protected VeloFixture Fixture { get; } = new ();

    protected AdminController Sut { get; }
}
