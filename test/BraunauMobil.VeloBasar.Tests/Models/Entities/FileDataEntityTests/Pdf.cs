namespace BraunauMobil.VeloBasar.Tests.Models.Entities.FileDataEntityTests;

public class Pdf
{
    [Theory]
    [VeloAutoData]
    public void ShouldReturnCorrectEntity(string fileName, byte[] data)
    {
        //  Arrange
        
        //  Act
        FileDataEntity result = FileDataEntity.Pdf(fileName, data);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.ContentType.Should().Be(FileDataEntity.PdfContentType);
            result.Data.Should().BeSameAs(data);
            result.FileName.Should().BeSameAs(fileName);
        }
    }
}
