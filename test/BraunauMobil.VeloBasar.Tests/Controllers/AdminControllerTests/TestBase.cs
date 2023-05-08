using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new AdminController(AdminService.Object);
    }

    public void VerifyNoOtherCalls()
    {
        AdminService.VerifyNoOtherCalls();
    }

    protected Mock<IAdminService> AdminService { get; } = new ();

    protected Fixture Fixture { get; } = new ();

    protected AdminController Sut { get; }
}
