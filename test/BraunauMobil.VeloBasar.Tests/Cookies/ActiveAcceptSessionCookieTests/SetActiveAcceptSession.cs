using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.ActiveAcceptSessionCookieTests;

public class SetActiveAcceptSession
    : TestBase
{
    [Theory]
	[AutoData]
	public void CallsAppend(AcceptSessionEntity acceptSession)
	{
		//	Arrange
		ActiveAcceptSessionCookie sut = new (HttpContextAccessor);
        A.CallTo(() => ResponseCookies.Append(sut.Key, acceptSession.Id.ToString(), sut.Options)).DoesNothing();

        //	Act
        sut.SetActiveAcceptSession(acceptSession);

        //	Assert
        A.CallTo(() => ResponseCookies.Append(sut.Key, acceptSession.Id.ToString(), sut.Options)).MustHaveHappenedOnceExactly();
	}
}

