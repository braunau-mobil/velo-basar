namespace BraunauMobil.VeloBasar.Tests.Models.Entities.FileDataEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        FileDataEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.ContentType.Should().BeNull();
            sut.Data.Should().BeNull();
            sut.FileName.Should().BeNull();
            sut.Id.Should().Be(0);
        }
    }
}
