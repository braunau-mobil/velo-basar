using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SetupControllerTests;

public class InitialSetupConfirmed
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task InvalidModel_ReturnsView(InitializationConfiguration configuration)
    {
        //  Arrange
        configuration.AdminUserEMail = string.Empty;
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
            }
        };

        //  Act
        IActionResult result = await Sut.InitialSetupConfirmed(configuration);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(configuration);
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsCreateDatabaseAndInitializeDatabaseAndReturnsRedirecToHome(InitializationConfiguration configuration, string url)
    {
        //  Arrange
        Router.Setup(_ => _.ToHome())
            .Returns(url);
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
            }
        };

        //  Act
        IActionResult result = await Sut.InitialSetupConfirmed(configuration);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        SetupService.Verify(_ => _.CreateDatabaseAsync(), Times.Once());
        SetupService.Verify(_ => _.InitializeDatabaseAsync(configuration), Times.Once());
        Router.Verify(_ => _.ToHome(), Times.Once);
        VerifyNoOtherCalls();
    }
}
