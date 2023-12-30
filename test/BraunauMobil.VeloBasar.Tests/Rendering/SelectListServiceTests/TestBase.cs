using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new SelectListService(Db, new StringLocalizerMock<SharedResources>());
    }

    public SelectListService Sut { get; }
}
