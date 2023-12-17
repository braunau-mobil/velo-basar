using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using BraunauMobil.VeloBasar.Tests.Mockups;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new (TransactionService.Object, Router.Object, SignInManager, new TransactionSuccessModelValidator(Localizer));

        Router.Setup(_ => _.Cancel)
            .Returns(CancelRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        CancelRouter.VerifyNoOtherCalls();
        TransactionService.VerifyNoOtherCalls();
    }

    protected Mock<ICancelRouter> CancelRouter { get; } = new ();

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected SignInManagerMock SignInManager { get; } = new ();

    protected TransactionController Sut { get; }

    protected Mock<ITransactionService> TransactionService { get; } = new();
}
