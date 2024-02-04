using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new BasarStatsService(ColorProvider, Db, X.FormatProvider);
    }

    public IColorProvider ColorProvider { get; } = X.StrictFake<IColorProvider>();

    public BasarStatsService Sut { get; }
}
