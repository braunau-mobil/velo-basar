using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        Sut = new TransactionService(NumberService, StatusPushService, Db, DocumentService, Clock, X.StringLocalizer, X.FormatProvider);
    }

    public IDocumentService DocumentService { get; } = X.StrictFake<IDocumentService>();

    public VeloFixture Fixture { get; } = new();

    public INumberService NumberService { get; } = X.StrictFake<INumberService>();
    
    public TransactionService Sut { get; }

    public IStatusPushService StatusPushService { get; } = X.StrictFake<IStatusPushService>();
}
