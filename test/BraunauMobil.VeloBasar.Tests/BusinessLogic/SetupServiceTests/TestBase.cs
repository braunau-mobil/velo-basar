using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SetupServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new SetupService(Db, new UserManagerMock(), X.StringLocalizer);
    }

    public SetupService Sut { get; }
}
