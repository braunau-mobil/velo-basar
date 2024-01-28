namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AbstractEntityTests;

public class Ctor
{
    private class TestEntity
        : AbstractEntity
    { }

    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        TestEntity sut = new();

        //  Assert
        sut.Id.Should().Be(0);
    }
}

