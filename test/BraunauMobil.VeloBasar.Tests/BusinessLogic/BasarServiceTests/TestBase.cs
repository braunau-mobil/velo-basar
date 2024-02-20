using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new BasarService(Db, StatsService, X.StringLocalizer, Clock);
    }

    public IBasarStatsService StatsService { get; } = X.StrictFake<IBasarStatsService>();

    public BasarService Sut { get; set; }
}
