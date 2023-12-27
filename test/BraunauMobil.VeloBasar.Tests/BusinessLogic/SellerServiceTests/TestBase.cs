using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new SellerService(TransactionService.Object, ProductLabelService.Object, StatusPushService.Object, TokenProvider.Object, Clock, Db);
    }

    public void VerifyNoOtherCalls()
    {
        ProductLabelService.VerifyNoOtherCalls();
        TokenProvider.VerifyNoOtherCalls();
        TransactionService.VerifyNoOtherCalls();
        StatusPushService.VerifyNoOtherCalls();
    }
    
    public SellerService Sut { get; }

    public Mock<IProductLabelService> ProductLabelService { get; } = new();

    public Mock<IStatusPushService> StatusPushService { get; } = new();

    public Mock<ITokenProvider> TokenProvider { get; } = new ();

    public Mock<ITransactionService> TransactionService { get; } = new();
}
