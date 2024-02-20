using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.CountryServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new CountryService(Db);
    }

    protected CountryService Sut { get; }
}
