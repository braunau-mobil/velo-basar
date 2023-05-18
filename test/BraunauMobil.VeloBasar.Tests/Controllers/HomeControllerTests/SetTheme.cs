using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.HomeControllerTests;

public class SetTheme
    : TestBase
{ 
    [Theory]
    [AutoData]
    public void SetsThemeAndRedirectsToReferer(Theme theme, string url)
    {
        //  Arrange
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        Sut.HttpContext.Request.Headers.Referer = url;

        //  Act
        IActionResult result = Sut.SetTheme(theme);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        CurrentThemeCookie.Verify(_ => _.SetCurrentTheme(theme), Times.Once());
        VerifyNoOtherCalls();
    }
}
