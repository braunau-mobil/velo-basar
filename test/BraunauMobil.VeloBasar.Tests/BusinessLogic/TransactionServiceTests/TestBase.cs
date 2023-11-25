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
        Sut = new TransactionService(NumberService.Object, TransactionDocumentService.Object, StatusPushService.Object, Db, ProductLabelService.Object, Clock.Object, Helpers.CreateActualLocalizer());
    }

    public void VerifyNoOtherCalls()
    {
        NumberService.VerifyNoOtherCalls();
        ProductLabelService.VerifyNoOtherCalls();
        StatusPushService.VerifyNoOtherCalls();
        TransactionDocumentService.VerifyNoOtherCalls();
    }
    
    public Fixture Fixture { get; } = new();

    public Mock<INumberService> NumberService { get; } = new();
    
    public TransactionService Sut { get; }

    public Mock<IProductLabelService> ProductLabelService { get; } = new();

    public Mock<IStatusPushService> StatusPushService { get; } = new();

    public Mock<ITransactionDocumentService> TransactionDocumentService { get; } = new();
}
