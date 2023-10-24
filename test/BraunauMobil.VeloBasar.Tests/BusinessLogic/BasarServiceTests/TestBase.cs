using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new BasarService(Db, ColorProvider.Object);
    }

    public void VerifyNoOtherCalls()
    {
        ColorProvider.VerifyNoOtherCalls();
    }

    public BasarService Sut { get; set; }

    public Mock<IColorProvider> ColorProvider { get; } = new();
}
