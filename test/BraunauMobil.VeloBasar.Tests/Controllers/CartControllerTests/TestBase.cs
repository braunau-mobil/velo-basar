using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class TestBase
{
    public TestBase()
    {
        Validator = new(ProductService.Object, TransactionService.Object, Router.Object, Localizer);
        Sut = new CartController(ProductService.Object, TransactionService.Object, Router.Object, Validator, Cookie.Object);

        Router.Setup(_ => _.Cancel)
            .Returns(CancelRouter.Object);
        Router.Setup(_ => _.Transaction)
            .Returns(TransactionRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        CancelRouter.VerifyNoOtherCalls();
        Cookie.VerifyNoOtherCalls();
        ProductService.VerifyNoOtherCalls();
        TransactionRouter.VerifyNoOtherCalls();
        TransactionService.VerifyNoOtherCalls();
    }

    protected Mock<ICancelRouter> CancelRouter { get; } = new();

    protected Mock<ICartCookie> Cookie { get; } = new();

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected Mock<IProductService> ProductService { get; } = new ();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected CartController Sut { get; }

    protected Mock<ITransactionRouter> TransactionRouter { get; } = new();

    protected Mock<ITransactionService> TransactionService { get; } = new ();
    
    protected CartModelValidator Validator { get; }

}
