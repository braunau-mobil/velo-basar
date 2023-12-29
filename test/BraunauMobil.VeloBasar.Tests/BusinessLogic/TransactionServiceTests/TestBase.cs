using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;
using FluentAssertions.Execution;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new TransactionService(NumberService, TransactionDocumentService, StatusPushService, Db, ProductLabelService, Clock, new StringLocalizerMock<SharedResources>());
    }
    
    public Fixture Fixture { get; } = new();

    public INumberService NumberService { get; } = X.StrictFake<INumberService>();
    
    public TransactionService Sut { get; }

    public IProductLabelService ProductLabelService { get; } = X.StrictFake<IProductLabelService>();

    public IStatusPushService StatusPushService { get; } = X.StrictFake<IStatusPushService>();

    public ITransactionDocumentService TransactionDocumentService { get; } = X.StrictFake<ITransactionDocumentService>();
}
