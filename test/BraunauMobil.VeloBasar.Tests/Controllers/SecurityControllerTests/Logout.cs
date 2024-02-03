using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SecurityControllerTests;

public class Logout
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldSignOutAndReturnRedirectToHome(string url)
    {
        //  Arrange
        IAuthenticationService authenticationService = X.StrictFake<IAuthenticationService>();
        A.CallTo(() => authenticationService.SignOutAsync(Sut.HttpContext, IdentityConstants.ApplicationScheme, null)).DoesNothing();
        A.CallTo(() => RequestServices.GetService(typeof(IAuthenticationService))).Returns(authenticationService);
        A.CallTo(() => Router.ToHome()).Returns(url);
        SignInManager.Context = Sut.HttpContext;

        //  Act
        IActionResult result = await Sut.Logout();

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }
        A.CallTo(() => authenticationService.SignOutAsync(Sut.HttpContext, IdentityConstants.ApplicationScheme, null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => RequestServices.GetService(typeof(IAuthenticationService))).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.ToHome()).MustHaveHappenedOnceExactly();
    }
}
