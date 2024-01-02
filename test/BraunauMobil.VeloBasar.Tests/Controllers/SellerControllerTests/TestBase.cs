using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class TestBase
{
    public TestBase()
    {
        A.CallTo(() => Router.AcceptSession).Returns(AcceptSessionRouter);
        A.CallTo(() => Router.Seller).Returns(SellerRouter);
        A.CallTo(() => Router.Transaction).Returns(TransactionRouter);

        Sut = new(SellerService, new SellerSearchModelValidator(Localizer), Router, SellerRouter, new Crud.SellerCrudModelFactory(Localizer, X.StrictFake<IHtmlHelper>(), SellerRouter, X.StrictFake<IVeloHtmlFactory>()), new SellerEntityValidator(Localizer));
    }

    protected IAcceptSessionRouter AcceptSessionRouter { get; } = X.StrictFake<IAcceptSessionRouter>();

    protected VeloFixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = new StringLocalizerMock<SharedResources>();

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected SellerController Sut { get; }

    protected ISellerRouter SellerRouter { get; } = X.StrictFake<ISellerRouter>();

    protected ISellerService SellerService { get; } = X.StrictFake<ISellerService>();

    protected ITransactionRouter TransactionRouter { get; } = X.StrictFake<ITransactionRouter>();
}
