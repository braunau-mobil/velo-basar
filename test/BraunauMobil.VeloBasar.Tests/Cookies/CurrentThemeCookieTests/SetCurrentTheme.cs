using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CurrentThemeCookieTests;

public class SetCurrentTheme
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public void CallsAppend(Theme theme)
    {
        //  Arrange
        CurrentThemeCookie sut = new (HttpContextAccessor);
        A.CallTo(() => ResponseCookies.Append(sut.Key, $"{theme}", sut.Options)).DoesNothing();

        //  Act
        sut.SetCurrentTheme(theme);

        //  Asert
        A.CallTo(() => ResponseCookies.Append(sut.Key, $"{theme}", sut.Options)).MustHaveHappenedOnceExactly();
    }
}
