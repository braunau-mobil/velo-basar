using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SetupControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new (SetupService.Object, Router.Object, new InitializationConfigurationValidator(Localizer));
    }

    public void VerifyNoOtherCalls()
    {
        SetupService.VerifyNoOtherCalls();
        Router.VerifyNoOtherCalls();
    }

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected SetupController Sut { get; }

    protected Mock<ISetupService> SetupService { get; } = new();
}
