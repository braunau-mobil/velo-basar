using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests.Pdf;

public class PdfExtensionsTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 28.346457F)]
    [InlineData(-10, -28.346457F)]
    public void ToUnit_Float_ShouldConvertMmToUnit(float mm, float expectedResult)
    {
        // Arrange

        // Act
        float result = PdfExtensions.ToUnit(mm);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 28.346457F)]
    [InlineData(-10, -28.346457F)]
    public void ToUnit_Int_ShouldConvertMmToUnit(int mm, float expectedResult)
    {
        // Arrange

        // Act
        float result = PdfExtensions.ToUnit(mm);

        // Assert
        result.Should().Be(expectedResult);
    }
}
