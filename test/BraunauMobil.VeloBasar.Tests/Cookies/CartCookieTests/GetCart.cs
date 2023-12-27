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
		CartCookie sut = new(HttpContextAccessor);
		A.CallTo(() => RequestCookies[sut.Key]).Returns(cookieValue);

		//	Act
		IList<int> result = sut.GetCart();

        //	Assert
        result.Should().BeEmpty();
        A.CallTo(() => RequestCookies[sut.Key]).MustHaveHappenedOnceExactly();
	}

    [Theory]
    [AutoData]
    public void ReturnsId(List<int> input)
    {
        //	Arrange
        string cookieValue = JsonSerializer.Serialize(input);
        CartCookie sut = new(HttpContextAccessor);
        A.CallTo(() => RequestCookies[sut.Key]).Returns(cookieValue);

        //	Act
        IList<int> result = sut.GetCart();

        //	Assert
        result.Should().BeEquivalentTo(input);
        A.CallTo(() => RequestCookies[sut.Key]).MustHaveHappenedOnceExactly();
    }
}

