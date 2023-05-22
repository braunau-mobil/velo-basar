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
		ActiveAcceptSessionCookie sut = new (HttpContextAccessor.Object);

		//	Act
		sut.SetActiveAcceptSession(acceptSession);

		//	Assert
		ResponseCookies.Verify(_ => _.Append(sut.Key, acceptSession.Id.ToString(), sut.Options));
		VerifyNoOtherCalls();
	}
}

