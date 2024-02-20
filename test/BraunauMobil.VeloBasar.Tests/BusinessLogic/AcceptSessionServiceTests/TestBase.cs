using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new AcceptSessionService(Db, TransactionService, Clock);
    }
    
    public AcceptSessionService Sut { get; }

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
