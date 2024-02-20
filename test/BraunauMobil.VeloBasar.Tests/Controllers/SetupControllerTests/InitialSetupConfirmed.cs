using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SetupControllerTests;

public class InitialSetupConfirmed
    : TestBase
{
    [Theory]
    [VeloAutoData]
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
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(configuration);
            view.ViewData.ModelState.IsValid.Should().BeFalse();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidModel_CallsInitializeDatabaseAsyncAndReturnsRedirecToHome(InitializationConfiguration configuration, string url)
    {
        //  Arrange
        A.CallTo(() => SetupService.MigrateDatabaseAsync()).DoesNothing();
        A.CallTo(() => SetupService.InitializeDatabaseAsync(configuration)).DoesNothing();
        A.CallTo(() => Router.ToHome()).Returns(url);
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
            }
        };

        //  Act
        IActionResult result = await Sut.InitialSetupConfirmed(configuration);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => SetupService.MigrateDatabaseAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => SetupService.InitializeDatabaseAsync(configuration)).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.ToHome()).MustHaveHappenedOnceExactly();
    }
}
