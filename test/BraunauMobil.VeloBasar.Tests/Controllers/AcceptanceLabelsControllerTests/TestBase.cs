using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptanceLabelsControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new AcceptanceLabelsController(TransactionService.Object, Localizer.Object);
    }

    public void VerifyNoOtherCalls()
    {
        TransactionService.VerifyNoOtherCalls();
        Localizer.VerifyNoOtherCalls();
    }

    protected Mock<IStringLocalizer<SharedResources>> Localizer { get; } = new ();

    protected Fixture Fixture { get; } = new ();

    protected Mock<ITransactionService> TransactionService { get; } = new ();

    protected AcceptanceLabelsController Sut { get; }
}
