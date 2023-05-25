using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class TestBase
    : SqliteTestBase
{
    public TestBase()
    {
        Sut = new AcceptProductService(Db, Helpers.CreateActualLocalizer());
    }
    
    public AcceptProductService Sut { get; }
}
