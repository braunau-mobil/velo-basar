namespace BraunauMobil.VeloBasar.Tests.Models.Entities.BasarEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        BasarEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Date.Should().Be(DateTime.MinValue);
            sut.Location.Should().BeNull();
            sut.Name.Should().BeNull();
            sut.ProductCommission.Should().Be(0);
        }
    }
}
