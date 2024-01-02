using BraunauMobil.VeloBasar.Cookies;
using System.Text.Json;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CartCookieTests;

public class SetCart
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public void CallsAppend(IList<int> cart)
    {
        //	Arrange
        string json = JsonSerializer.Serialize(cart);
        CartCookie sut = new (HttpContextAccessor);
        A.CallTo(() => ResponseCookies.Append(sut.Key, json, sut.Options)).DoesNothing();

        //	Act
        sut.SetCart(cart);

        //	Assert
        A.CallTo(() => ResponseCookies.Append(sut.Key, json, sut.Options)).MustHaveHappenedOnceExactly();
    }
}
