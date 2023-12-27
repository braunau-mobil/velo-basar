using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new AdminController(AdminService.Object, Clock);
    }

    public void VerifyNoOtherCalls()
    {
        AdminService.VerifyNoOtherCalls();
    }

    protected Mock<IAdminService> AdminService { get; } = new ();

    protected ClockMock Clock { get; } = new();

    protected Fixture Fixture { get; } = new ();

    protected AdminController Sut { get; }
}
