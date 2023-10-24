using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class TestBase
    : SqliteTestBase
{
    public TestBase()
    {
        Sut = new SellerService(TransactionService.Object, ProductLabelService.Object, TokenProvider.Object, Clock.Object, Db);
    }

    public override void Dispose()
    {
        VerifyNoOtherCalls();
        base.Dispose();
    }

    public void VerifyNoOtherCalls()
    {
        ProductLabelService.VerifyNoOtherCalls();
        TokenProvider.VerifyNoOtherCalls();
        TransactionService.VerifyNoOtherCalls();
    }
    
    public SellerService Sut { get; }

    public Mock<IProductLabelService> ProductLabelService { get; } = new();

    public Mock<ITokenProvider> TokenProvider { get; } = new ();

    public Mock<ITransactionService> TransactionService { get; } = new();
}
