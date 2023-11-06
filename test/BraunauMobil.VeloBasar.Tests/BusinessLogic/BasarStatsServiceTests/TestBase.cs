using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new BasarStatsService(ColorProvider.Object, Db);
    }

    public void VerifyNoOtherCalls()
    {
        ColorProvider.VerifyNoOtherCalls();
    }

    public Mock<IColorProvider> ColorProvider { get; } = new();

    public BasarStatsService Sut { get; }
}
