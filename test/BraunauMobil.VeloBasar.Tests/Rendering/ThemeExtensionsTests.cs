using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering;

public class ThemeExtensionsTests
{
    [Theory]
    [InlineData(Theme.DefaultLight, "/css/defaul_-light.css")]
    [InlineData(Theme.DefaultDark, "/css/default_dark.css")]
    [InlineData(Theme.Brutal, "/css/neo_brutal.css")]
    public void CssFilePath_ShouldReturnCorrectCssFilePath(Theme theme, string expectedCssFilePath)
    {
        // Arrange

        // Act
        string actualCssFilePath = theme.CssFilePath();

        // Assert
        actualCssFilePath.Should().Be(expectedCssFilePath);
    }
}
