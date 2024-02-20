using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductTypeServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new ProductTypeService(Db);
    }

    protected ProductTypeService Sut { get; }
}
