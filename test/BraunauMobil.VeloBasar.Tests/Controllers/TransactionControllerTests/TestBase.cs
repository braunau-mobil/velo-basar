using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class TestBase
{
    public TestBase()
    {
        A.CallTo(() => Router.Cancel).Returns(CancelRouter);
        A.CallTo(() => Router.Transaction).Returns(TransactionRouter);

        Sut = new (TransactionService, Router, SignInManager, new TransactionSuccessModelValidator(Localizer));        
    }

    protected ICancelRouter CancelRouter { get; } = X.StrictFake<ICancelRouter>();

    protected VeloFixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = X.StringLocalizer;

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected SignInManagerMock SignInManager { get; } = new ();

    protected TransactionController Sut { get; }

    protected ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();

    protected ITransactionRouter TransactionRouter { get; } = X.StrictFake<ITransactionRouter>();
}
