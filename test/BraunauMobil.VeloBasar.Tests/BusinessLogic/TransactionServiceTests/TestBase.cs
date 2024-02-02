using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new TransactionService(NumberService, StatusPushService, Db, DocumentService, Clock, new StringLocalizerMock<SharedResources>());
    }

    public IDocumentService DocumentService { get; } = X.StrictFake<IDocumentService>();

    public VeloFixture Fixture { get; } = new();

    public INumberService NumberService { get; } = X.StrictFake<INumberService>();
    
    public TransactionService Sut { get; }

    public IStatusPushService StatusPushService { get; } = X.StrictFake<IStatusPushService>();
}
