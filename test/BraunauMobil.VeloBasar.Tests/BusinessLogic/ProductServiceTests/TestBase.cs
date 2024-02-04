using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new ProductService(Db, DocumentService, TransactionService, X.StringLocalizer);
    }
    
    public IDocumentService DocumentService { get; } = X.StrictFake<IDocumentService>();

    public ProductService Sut { get; }

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
