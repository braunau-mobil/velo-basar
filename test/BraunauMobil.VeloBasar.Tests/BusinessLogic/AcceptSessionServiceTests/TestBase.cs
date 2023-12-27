using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new AcceptSessionService(Db, TransactionService, Clock);
    }
    
    public AcceptSessionService Sut { get; }

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
