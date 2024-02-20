using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new SellerService(TransactionService, DocumentService, StatusPushService, TokenProvider, Clock, Db, X.StringLocalizer);
    }
    
    public IDocumentService DocumentService { get; } = X.StrictFake<IDocumentService>();
    
    public SellerService Sut { get; }

    public IStatusPushService StatusPushService { get; } = X.StrictFake<IStatusPushService>();

    public ITokenProvider TokenProvider { get; } = X.StrictFake<ITokenProvider>();

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
