using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar.Tests.Cookies.ActiveAcceptSessionCookieTests;

public class GetActiveAcceptSessionId
	: TestBase
{
	[Theory]
	[InlineData(null)]
    [InlineData("")]
	[AutoData]
    public void ReturnsNull(string? cookieValue)
	{
		//	Arrange
		ActiveAcceptSessionCookie sut = new(HttpContextAccessor.Object);
		RequestCookies.SetupGet(_ => _[sut.Key])
			.Returns(cookieValue);

		//	Act
		int? result = sut.GetActiveAcceptSessionId();

		//	Assert
		result.Should().BeNull();
		RequestCookies.VerifyGet(_ => _[sut.Key], Times.Once());
		VerifyNoOtherCalls();
	}

    [Theory]
    [InlineData("123")]
    public void ReturnsId(string? cookieValue)
    {
        //	Arrange
        ActiveAcceptSessionCookie sut = new(HttpContextAccessor.Object);
        RequestCookies.SetupGet(_ => _[sut.Key])
            .Returns(cookieValue);

        //	Act
        int? result = sut.GetActiveAcceptSessionId();

        //	Assert
        result.Should().Be(123);
        RequestCookies.VerifyGet(_ => _[sut.Key], Times.Once());
        VerifyNoOtherCalls();
    }
}

