using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SecurityControllerTests;

public class Login
    : TestBase
{
    [Fact]
    public async Task ShouldSignOutAndReturnViewWithLoginModel()
    {
        //  Arrange
        IAuthenticationService authenticationService = X.StrictFake<IAuthenticationService>();
        A.CallTo(() => authenticationService.SignOutAsync(Sut.HttpContext, IdentityConstants.ExternalScheme, null)).DoesNothing();
        A.CallTo(() => RequestServices.GetService(typeof(IAuthenticationService)))
            .Returns(authenticationService);        

        //  Act
        IActionResult result = await Sut.Login();

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<LoginModel>();
        }
        A.CallTo(() => authenticationService.SignOutAsync(Sut.HttpContext, IdentityConstants.ExternalScheme, null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => RequestServices.GetService(typeof(IAuthenticationService))).MustHaveHappenedOnceExactly();
    }
}
