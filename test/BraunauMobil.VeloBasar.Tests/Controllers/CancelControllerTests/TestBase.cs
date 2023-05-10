using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CancelControllerTests;

public class TestBase
{
    public TestBase()
    {
        SelectSaleValidator = new(Localizer);
        SelectProductsValidator = new(Localizer);
        Sut = new CancelController(Router.Object, TransactionService.Object, SelectSaleValidator, SelectProductsValidator);

        Router.Setup(_ => _.Cancel)
            .Returns(CancelRouter.Object);
        Router.Setup(_ => _.Transaction)
            .Returns(TransactionRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        CancelRouter.VerifyNoOtherCalls();
        TransactionRouter.VerifyNoOtherCalls();
        TransactionService.VerifyNoOtherCalls();
    }

    protected Mock<ICancelRouter> CancelRouter { get; } = new();

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected CancelController Sut { get; }

    protected SelectSaleModelValidator SelectSaleValidator { get; }

    protected SelectProductsModelValidator SelectProductsValidator { get; }

    protected Mock<ITransactionRouter> TransactionRouter { get; } = new();

    protected Mock<ITransactionService> TransactionService { get; } = new ();

}
