using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new BasarService(Db, StatsService.Object, Helpers.CreateActualLocalizer(), Clock.Object);
    }

    public void VerifyNoOtherCalls()
    {
        StatsService.VerifyNoOtherCalls();
    }

    public Mock<IBasarStatsService> StatsService { get; } = new();
    public BasarService Sut { get; set; }
}
