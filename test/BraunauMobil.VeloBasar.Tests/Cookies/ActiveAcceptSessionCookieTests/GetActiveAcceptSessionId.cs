using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.ActiveAcceptSessionCookieTests;

public class GetActiveAcceptSessionId
	: TestBase
{
	[Theory]
	[InlineData(null)]
    [InlineData("")]
	[VeloAutoData]
    public void ReturnsNull(string? cookieValue)
	{
		//	Arrange
		ActiveAcceptSessionCookie sut = new(HttpContextAccessor);
        A.CallTo(() => RequestCookies[sut.Key]).Returns(cookieValue);

		//	Act
		int? result = sut.GetActiveAcceptSessionId();

		//	Assert
		result.Should().BeNull();
        A.CallTo(() => RequestCookies[sut.Key]).MustHaveHappenedOnceExactly();
	}

    [Theory]
    [InlineData("123")]
    public void ReturnsId(string? cookieValue)
    {
        //	Arrange
        ActiveAcceptSessionCookie sut = new(HttpContextAccessor);
        A.CallTo(() => RequestCookies[sut.Key]).Returns(cookieValue);

        //	Act
        int? result = sut.GetActiveAcceptSessionId();

        //	Assert
        result.Should().Be(123);
        A.CallTo(() => RequestCookies[sut.Key]).MustHaveHappenedOnceExactly();
    }
}

