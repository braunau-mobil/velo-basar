using BraunauMobil.VeloBasar.Extensions;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.Extensions.JsonUtilsTests;

public class ToChartJsColor
{
    [Theory]
    [InlineData(11, 22, 33, 44, "rgb(22, 33, 44)")]
    public void ShouldReturnJson(byte a, byte r, byte g, byte b, string expectedResult)
    {
        //  Arrange
        Color color = Color.FromArgb(a, r, g, b);

        //  Act
        string result = JsonUtils.ToChartJsColor(color);

        //  Assert
        result.Should().Be(expectedResult);
    }
}
