using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CartCookieTests;

public class ClearCart
	: TestBase
{
    [Fact]
	public void CallsDelete()
	{
		//	Arrange
		CartCookie sut = new (HttpContextAccessor.Object);

		//	Act
		sut.ClearCart();

		//	Assert
		ResponseCookies.Verify(_ => _.Delete(sut.Key, sut.Options));
		VerifyNoOtherCalls();
	}
}

