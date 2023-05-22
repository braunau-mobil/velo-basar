using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.ActiveAcceptSessionCookieTests;

public class ClearActiveAcceptSession
	: TestBase
{
    [Fact]
	public void CallsDelete()
	{
		//	Arrange
		ActiveAcceptSessionCookie sut = new (HttpContextAccessor.Object);

		//	Act
		sut.ClearActiveAcceptSession();

		//	Assert
		ResponseCookies.Verify(_ => _.Delete(sut.Key, sut.Options));
		VerifyNoOtherCalls();
	}
}

