using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new ProductService(Db, DocumentService, TransactionService, X.StringLocalizer);
    }
    
    public IDocumentService DocumentService { get; } = X.StrictFake<IDocumentService>();

    public ProductService Sut { get; }

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
