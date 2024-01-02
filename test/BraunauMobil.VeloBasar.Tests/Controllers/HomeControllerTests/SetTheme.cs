using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.HomeControllerTests;

public class SetTheme
    : TestBase
{ 
    [Theory]
    [VeloAutoData]
    public void SetsThemeAndRedirectsToReferer(Theme theme, string url)
    {
        //  Arrange
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        Sut.HttpContext.Request.Headers.Referer = url;
        A.CallTo(() => CurrentThemeCookie.SetCurrentTheme(theme)).DoesNothing();

        //  Act
        IActionResult result = Sut.SetTheme(theme);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => CurrentThemeCookie.SetCurrentTheme(theme)).MustHaveHappenedOnceExactly();
    }
}
