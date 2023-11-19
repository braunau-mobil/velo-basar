using BraunauMobil.VeloBasar.Cookies;
using System.Text.Json;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CartCookieTests;

public class SetCart
    : TestBase
{
    [Theory]
    [AutoData]
    public void CallsAppend(IList<int> cart)
    {
        //	Arrange
        string json = JsonSerializer.Serialize(cart);
        CartCookie sut = new (HttpContextAccessor.Object);

        //	Act
        sut.SetCart(cart);

        //	Assert
        ResponseCookies.Verify(_ => _.Append(sut.Key, json, sut.Options));
        VerifyNoOtherCalls();
    }
}
