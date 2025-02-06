using System.IO;
using AutoFixture.Xunit2;
using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Pdf;
using iText.Kernel.Pdf;
using iText.Layout;

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
        float result = mm.ToUnit();

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
        float result = mm.ToUnit();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [AutoData]
    public void SetMargins_ShouldSetMarginsCorrectly(Margins margins)
    {
        //  Arrange
        using MemoryStream memoryStream = new();
        using PdfWriter pdfWriter = new(memoryStream);
        using PdfDocument pdfDoc = new(pdfWriter);
        using Document doc = new(pdfDoc);
        
        //  Act
        doc.SetMargins(margins);
        
        //  Assert
        using (new AssertionScope())
        {
            doc.GetLeftMargin().Should().Be(margins.Left.ToUnit());
            doc.GetTopMargin().Should().Be(margins.Top.ToUnit());
            doc.GetRightMargin().Should().Be(margins.Right.ToUnit());
            doc.GetBottomMargin().Should().Be(margins.Bottom.ToUnit());
        }
    }
}
