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
        Sut = new (SellerService.Object, new SellerSearchModelValidator(Localizer), Router.Object, SellerRouter.Object, new Crud.SellerCrudModelFactory(Localizer, Mock.Of<IHtmlHelper>(), SellerRouter.Object, new Mock<IVeloHtmlFactory>().Object), new SellerEntityValidator(Localizer));

        Router.Setup(_ => _.AcceptSession)
            .Returns(AcceptSessionRouter.Object);
        Router.Setup(_ => _.Seller)
            .Returns(SellerRouter.Object);
        Router.Setup(_ => _.Transaction)
            .Returns(TransactionRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        AcceptSessionRouter.VerifyNoOtherCalls();
        SellerRouter.VerifyNoOtherCalls();
        SellerService.VerifyNoOtherCalls();
        TransactionRouter.VerifyNoOtherCalls();
    }

    protected Mock<IAcceptSessionRouter> AcceptSessionRouter { get; } = new();

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected SellerController Sut { get; }

    protected Mock<ISellerRouter> SellerRouter { get; } = new();

    protected Mock<ISellerService> SellerService { get; } = new();

    protected Mock<ITransactionRouter> TransactionRouter { get; } = new();
}
