using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new BasarService(Db, StatsService, Helpers.CreateActualLocalizer(), Clock);
    }

    public IBasarStatsService StatsService { get; } = X.StrictFake<IBasarStatsService>();

    public BasarService Sut { get; set; }
}
