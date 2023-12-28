using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering;

public class TransactionTypeExtensionsTests
{
    [Theory]
    [InlineData(TransactionType.Acceptance, "light")]
    [InlineData(TransactionType.Cancellation, "info")]
    [InlineData(TransactionType.Lock, "danger")]
    [InlineData(TransactionType.SetLost, "warning")]
    [InlineData(TransactionType.Unlock, "secondary")]
    [InlineData(TransactionType.Sale, "success")]
    [InlineData(TransactionType.Settlement, "secondary")]
    public void ToCssColor_ShouldReturnCorrectCssColor(TransactionType transactionType, string expectedCssColor)
    {
        // Arrange

        // Act
        string actualCssColor = transactionType.ToCssColor();

        // Assert
        actualCssColor.Should().Be(expectedCssColor);
    }
}
