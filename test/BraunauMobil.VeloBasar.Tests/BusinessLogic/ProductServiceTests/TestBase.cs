using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new ProductService(Db, ProductLabelService.Object, TransactionService.Object);
    }

    public void VerifyNoOtherCalls()
    {
        ProductLabelService.VerifyNoOtherCalls();
        TransactionService.VerifyNoOtherCalls();
    }
    
    public ProductService Sut { get; }

    public Mock<IProductLabelService> ProductLabelService { get; } = new();

    public Mock<ITransactionService> TransactionService { get; } = new();
}
