using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CurrentThemeCookieTests;

public class GetCurrentTheme
    : TestBase
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [AutoData]
    public void ReturnsDefault(string? cookieValue)
    {
        //  Arrange
        CurrentThemeCookie sut = new (HttpContextAccessor);
        A.CallTo(() => RequestCookies[sut.Key]).Returns(cookieValue);

        //  Act
        Theme theme = sut.GetCurrentTheme();

        //  Asert
        theme.Should().Be(Theme.DefaultLight);
        A.CallTo(() => RequestCookies[sut.Key]).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("Brutal")]
    public void ReturnsTheme(string cookieValue)
    {
        //  Arrange
        CurrentThemeCookie sut = new(HttpContextAccessor);
        A.CallTo(() => RequestCookies[sut.Key]).Returns(cookieValue);

        //  Act
        Theme theme = sut.GetCurrentTheme();

        //  Asert
        theme.Should().Be(Theme.Brutal);
        A.CallTo(() => RequestCookies[sut.Key]).MustHaveHappenedOnceExactly();
    }
}
