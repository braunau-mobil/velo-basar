using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class TestBase<TDbFixture>
    : DbTestBase<TDbFixture>
    where TDbFixture : IDbFixture, new()
{
    public TestBase()
    {
        Sut = new SellerService(TransactionService, ProductLabelService, StatusPushService, TokenProvider, Clock, Db);
    }
    
    public SellerService Sut { get; }

    public IProductLabelService ProductLabelService { get; } = X.StrictFake<IProductLabelService>();

    public IStatusPushService StatusPushService { get; } = X.StrictFake<IStatusPushService>();

    public ITokenProvider TokenProvider { get; } = X.StrictFake<ITokenProvider>();

    public ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
