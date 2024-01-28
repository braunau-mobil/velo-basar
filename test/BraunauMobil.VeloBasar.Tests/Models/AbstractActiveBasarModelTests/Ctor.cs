namespace BraunauMobil.VeloBasar.Tests.Models.AbstractActiveBasarModelTests;

public class Ctor
{
    public class MyModel
        : AbstractActiveBasarModel
    {
    }

    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        MyModel sut = new();

        //  Assert
        sut.BasarId.Should().Be(0);
    }
}
