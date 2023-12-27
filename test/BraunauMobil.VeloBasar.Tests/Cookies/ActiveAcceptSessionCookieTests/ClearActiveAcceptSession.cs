using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.ActiveAcceptSessionCookieTests;

public class ClearActiveAcceptSession
	: TestBase
{
    [Fact]
	public void CallsDelete()
	{
		//	Arrange
		ActiveAcceptSessionCookie sut = new (HttpContextAccessor);
        A.CallTo(() => ResponseCookies.Delete(sut.Key, sut.Options)).DoesNothing();

        //	Act
        sut.ClearActiveAcceptSession();

        //	Assert
        A.CallTo(() => ResponseCookies.Delete(sut.Key, sut.Options)).MustHaveHappenedOnceExactly();
	}
}

