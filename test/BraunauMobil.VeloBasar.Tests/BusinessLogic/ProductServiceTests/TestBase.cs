using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new ProductService(Db, ProductLabelService, TransactionService);
    }
    
    public ProductService Sut { get; }

    public IProductLabelService ProductLabelService { get; } = X.StrictFake<IProductLabelService>();

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
