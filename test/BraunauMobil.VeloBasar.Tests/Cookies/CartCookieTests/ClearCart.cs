using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CartCookieTests;

public class ClearCart
	: TestBase
{
    [Fact]
	public void CallsDelete()
	{
		//	Arrange
		CartCookie sut = new (HttpContextAccessor);
        A.CallTo(() => ResponseCookies.Delete(sut.Key, sut.Options)).DoesNothing();

        //	Act
        sut.ClearCart();

        //	Assert
        A.CallTo(() => ResponseCookies.Delete(sut.Key, sut.Options)).MustHaveHappenedOnceExactly();
	}
}
