using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class TestBase
{
    public TestBase()
    {
        ProductAnnotateModelValidator = new (Localizer);
        ProductEntityValidator = new(Localizer);

        A.CallTo(() => Router.Product).Returns(ProductRouter);

        Sut = new (Router, ProductService, ProductAnnotateModelValidator, ProductEntityValidator);        
    }

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected IProductService ProductService { get; } = X.StrictFake<IProductService>();

    protected ProductAnnotateModelValidator ProductAnnotateModelValidator { get; }

    protected ProductEntityValidator ProductEntityValidator { get; }

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected ProductController Sut { get; }

    protected IProductRouter ProductRouter { get; } = X.StrictFake<IProductRouter>();
}
