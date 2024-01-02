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
        Validator = new(ProductService, TransactionService, Router, Localizer);

        A.CallTo(() => Router.Cancel).Returns(CancelRouter);
        A.CallTo(() => Router.Transaction).Returns(TransactionRouter);

        Sut = new CartController(ProductService, TransactionService, Router, Validator, Cookie);        
    }

    protected ICancelRouter CancelRouter { get; } = X.StrictFake<ICancelRouter>();

    protected ICartCookie Cookie { get; } = X.StrictFake<ICartCookie>();

    protected VeloFixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = new StringLocalizerMock<SharedResources>();

    protected IProductService ProductService { get; } = X.StrictFake<IProductService>();

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected CartController Sut { get; }

    protected ITransactionRouter TransactionRouter { get; } = X.StrictFake<ITransactionRouter>();

    protected ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
    
    protected CartModelValidator Validator { get; }

}
