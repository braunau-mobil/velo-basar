using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering;

public class MessageTypeExtensionsTests
{
    [Theory]
    [InlineData(MessageType.Success, "alert-success")]
    [InlineData(MessageType.Danger, "alert-danger")]
    [InlineData(MessageType.Warning, "alert-warning")]
    [InlineData(MessageType.Info, "alert-primary")]
    public void ToCss_ShouldReturnCorrectCssClass(MessageType messageType, string expectedCssClass)
    {
        // Arrange

        // Act
        string result = messageType.ToCss();

        // Assert
        result.Should().Be(expectedCssClass);
    }
}
