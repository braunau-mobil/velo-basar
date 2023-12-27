using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new AcceptSessionService(Db, TransactionService.Object, Clock);
    }

    public void VerifyNoOtherCalls()
    {
        TransactionService.VerifyNoOtherCalls();
    }
    
    public AcceptSessionService Sut { get; }

    public Mock<ITransactionService> TransactionService { get; } = new();
}
