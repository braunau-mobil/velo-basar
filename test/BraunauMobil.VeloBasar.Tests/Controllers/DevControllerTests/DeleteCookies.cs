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
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = Sut.DeleteCookies();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;
        
        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Fact]
    public void NoParameters_DevToolsEnabled_ReturnsView()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(true);

        //  Act
        IActionResult result = Sut.DeleteCookies();

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Fact]
    public void WithConfig_DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = Sut.DeleteCookiesConfirmed();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void WithConfig_DevToolsEnabled_ContextualizesServiceAndCallsGenerateAndClearsCookiesAndReturnsRedirectToHome(string url)
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(true);
        Router.Setup(_ => _.ToHome())
            .Returns(url);
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        //  Act
        IActionResult result = Sut.DeleteCookiesConfirmed();

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        Router.Verify(_ => _.ToHome(), Times.Once());
        VerifyNoOtherCalls();
    }
}
