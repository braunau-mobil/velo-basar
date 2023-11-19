using BraunauMobil.VeloBasar.Cookies;
using System.Text.Json;

namespace BraunauMobil.VeloBasar.Tests.Cookies.CartCookieTests;

public class GetCart
    : TestBase
{
	[Theory]
	[InlineData(null)]
    [InlineData("")]
    public void ReturnsEmptyList(string? cookieValue)
	{
		//	Arrange
		CartCookie sut = new(HttpContextAccessor.Object);
		RequestCookies.SetupGet(_ => _[sut.Key])
			.Returns(cookieValue);

		//	Act
		IList<int> result = sut.GetCart();

        //	Assert
        result.Should().BeEmpty();
		RequestCookies.VerifyGet(_ => _[sut.Key], Times.Once());
		VerifyNoOtherCalls();
	}

    [Theory]
    [AutoData]
    public void ReturnsId(List<int> input)
    {
        //	Arrange
        string cookieValue = JsonSerializer.Serialize(input);
        CartCookie sut = new(HttpContextAccessor.Object);
        RequestCookies.SetupGet(_ => _[sut.Key])
            .Returns(cookieValue);

        //	Act
        IList<int> result = sut.GetCart();

        //	Assert
        result.Should().BeEquivalentTo(input);
        RequestCookies.VerifyGet(_ => _[sut.Key], Times.Once());
        VerifyNoOtherCalls();
    }
}

