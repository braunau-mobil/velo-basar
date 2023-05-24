using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CurrentThemeCookieTests;

public class SetCurrentTheme
    : TestBase
{
    [Theory]
    [AutoData]
    public void CallsAppend(Theme theme)
    {
        //  Arrange
        CurrentThemeCookie sut = new (HttpContextAccessor.Object);

        //  Act
        sut.SetCurrentTheme(theme);

        //  Asert
        ResponseCookies.Verify(_ => _.Append(sut.Key, $"{theme}", sut.Options), Times.Once());
    }
}
