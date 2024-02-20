using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new AcceptProductService(Db, X.StringLocalizer);
    }
    
    public AcceptProductService Sut { get; }
}
