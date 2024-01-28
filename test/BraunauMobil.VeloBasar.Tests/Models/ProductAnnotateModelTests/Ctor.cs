namespace BraunauMobil.VeloBasar.Tests.Models.ProductAnnotateModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        ProductAnnotateModel sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Notes.Should().Be("");
            sut.ProductId.Should().Be(0);
        }
    }
}
