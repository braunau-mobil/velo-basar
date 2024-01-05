namespace BraunauMobil.VeloBasar.Tests.Models.Entities.FileDataEntityTests;

public class Csv
{
    [Theory]
    [VeloAutoData]
    public void ShouldReturnCorrectEntity(string fileName, byte[] data)
    {
        //  Arrange
        
        //  Act
        FileDataEntity result = FileDataEntity.Csv(fileName, data);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.ContentType.Should().Be(FileDataEntity.CsvContentType);
            result.Data.Should().BeSameAs(data);
            result.FileName.Should().BeSameAs(fileName);
        }
    }
}
