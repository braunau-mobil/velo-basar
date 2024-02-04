using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class TestBase
{
    public TestBase()
    {
        Validator = new (Localizer);
        A.CallTo(() => Router.AcceptProduct).Returns(AcceptProductRouter);

        Sut = new AcceptProductController(AcceptProductService, Router, Validator);
    }

    protected IAcceptProductRouter AcceptProductRouter { get; } = X.StrictFake<IAcceptProductRouter>();

    protected IAcceptProductService AcceptProductService { get; } = X.StrictFake<IAcceptProductService>();

    protected VeloFixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = X.StringLocalizer;
    
    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();
    
    protected AcceptProductController Sut { get; }

    protected ProductEntityValidator Validator { get; }
}
