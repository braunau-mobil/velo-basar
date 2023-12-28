using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering;

public class BadgeTypeExtensionsTests
{
    [Theory]
    [InlineData(BadgeType.Primary, "text-bg-primary")]
    [InlineData(BadgeType.Secondary, "text-bg-secondary")]
    [InlineData(BadgeType.Success, "text-bg-success")]
    [InlineData(BadgeType.Danger, "text-bg-danger")]
    [InlineData(BadgeType.Warning, "text-bg-warning")]
    [InlineData(BadgeType.Info, "text-bg-info")]
    [InlineData(BadgeType.Light, "text-bg-light")]
    [InlineData(BadgeType.Dark, "text-bg-dark")]
    public void ToCss_ShouldReturnCorrectCssClass(BadgeType type, string expectedCssClass)
    {
        // Arrange

        // Act
        string result = type.ToCss();

        // Assert
        result.Should().Be(expectedCssClass);
    }
}
