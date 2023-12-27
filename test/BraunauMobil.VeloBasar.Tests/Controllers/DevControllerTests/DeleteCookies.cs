using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class DeleteCookies
    : TestBase
{ 
    [Fact]
    public void NoParameters_DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(false);

        //  Act
        IActionResult result = Sut.DeleteCookies();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void NoParameters_DevToolsEnabled_ReturnsView()
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(true);

        //  Act
        IActionResult result = Sut.DeleteCookies();

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void WithConfig_DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(false);

        //  Act
        IActionResult result = Sut.DeleteCookiesConfirmed();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public void WithConfig_DevToolsEnabled_ContextualizesServiceAndCallsGenerateAndClearsCookiesAndReturnsRedirectToHome(string url)
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(true);
        A.CallTo(() => Router.ToHome()).Returns(url);
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        //  Act
        IActionResult result = Sut.DeleteCookiesConfirmed();

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.ToHome()).MustHaveHappenedOnceExactly();
    }
}
