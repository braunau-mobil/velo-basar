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
        A.CallTo(() => Router.Cancel).Returns(CancelRouter);
        A.CallTo(() => Router.Transaction).Returns(TransactionRouter);

        Sut = new CancelController(Router, TransactionService, SelectSaleValidator, SelectProductsValidator);        
    }

    protected ICancelRouter CancelRouter { get; } = X.StrictFake<ICancelRouter>();

    protected VeloFixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = X.StringLocalizer;

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected CancelController Sut { get; }

    protected SelectSaleModelValidator SelectSaleValidator { get; }

    protected SelectProductsModelValidator SelectProductsValidator { get; }

    protected ITransactionRouter TransactionRouter { get; } = X.StrictFake<ITransactionRouter>();

    protected ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();

}
