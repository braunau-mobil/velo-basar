using Microsoft.AspNetCore.Hosting;

namespace BraunauMobil.VeloBasar.Tests.VeloBasarAppContextTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new VeloBasarAppContext(WebHostEnvironment, Db);
    }

    protected VeloBasarAppContext Sut { get; }

    protected IWebHostEnvironment WebHostEnvironment { get; } = X.StrictFake<IWebHostEnvironment>();
}
