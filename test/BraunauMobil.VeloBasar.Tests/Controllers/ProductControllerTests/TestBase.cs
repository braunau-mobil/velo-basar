using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class TestBase
{
    public TestBase()
    {
        ProductAnnotateModelValidator = new (Localizer);
        ProductEntityValidator = new(Localizer);
        Sut = new (Router.Object, ProductService.Object, ProductAnnotateModelValidator, ProductEntityValidator);

        Router.Setup(_ => _.Product)
            .Returns(ProductRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        ProductService.VerifyNoOtherCalls();
        ProductRouter.VerifyNoOtherCalls();
    }

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected Mock<IProductService> ProductService { get; } = new ();

    protected ProductAnnotateModelValidator ProductAnnotateModelValidator { get; }

    protected ProductEntityValidator ProductEntityValidator { get; }

    protected Mock<IVeloRouter> Router { get; } = new();

    protected ProductController Sut { get; }

    protected Mock<IProductRouter> ProductRouter { get; } = new();
}
