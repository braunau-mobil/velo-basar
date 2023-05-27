using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class TestBase
    : SqliteTestBase
{
    public TestBase()
    {
        Sut = new AcceptSessionService(Db, TransactionService.Object, Clock.Object);
    }

    public void VerifyNoOtherCalls()
    {
        TransactionService.VerifyNoOtherCalls();
    }
    
    public AcceptSessionService Sut { get; }

    public Mock<ITransactionService> TransactionService { get; } = new();
}
