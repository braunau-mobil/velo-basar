using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SetupServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new SetupService(Db, new UserManagerMock());
    }

    public SetupService Sut { get; }
}
