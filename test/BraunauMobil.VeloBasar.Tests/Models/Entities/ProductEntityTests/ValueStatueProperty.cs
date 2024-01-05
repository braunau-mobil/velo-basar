namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class ValueStateProperty
{
    public void DefaultValueShouldBeNotSettled()
    {
        //  Arrange
        ProductEntity sut = new();

        //  Act
        ValueState result = sut.ValueState;

        //  Assert
        result.Should().Be(ValueState.NotSettled);
    }
}
