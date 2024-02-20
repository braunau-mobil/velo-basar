using Microsoft.AspNetCore.Hosting;

namespace BraunauMobil.VeloBasar.Tests.VeloBasarAppContextTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new VeloBasarAppContext(WebHostEnvironment, Db);
    }

    protected VeloBasarAppContext Sut { get; }

    protected IWebHostEnvironment WebHostEnvironment { get; } = X.StrictFake<IWebHostEnvironment>();
}
