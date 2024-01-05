namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class StorageStateProperty
{
    public void DefaultValueShouldBeNotAccepted()
    {
        //  Arrange
        ProductEntity sut = new();

        //  Act
        StorageState result = sut.StorageState;

        //  Assert
        result.Should().Be(StorageState.NotAccepted);
    }
}
