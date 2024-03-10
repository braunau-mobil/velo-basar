namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class WasPickedUp
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [InlineData(ValueState.Settled, StorageState.Available)]
    [InlineData(ValueState.Settled, StorageState.Locked)]
    public void ShouldReturnTrue(ValueState valueState, StorageState storageState)
    {
        // Arrange
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.ValueState, valueState)
            .With(product => product.StorageState, storageState)
            .Create();

        // Act
        bool result = product.WasPickedUp;

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(ValueState.NotSettled, StorageState.Available)]
    [InlineData(ValueState.NotSettled, StorageState.Locked)]
    [InlineData(ValueState.NotSettled, StorageState.Lost)]
    [InlineData(ValueState.NotSettled, StorageState.NotAccepted)]
    [InlineData(ValueState.NotSettled, StorageState.Sold)]
    [InlineData(ValueState.Settled, StorageState.Lost)]
    [InlineData(ValueState.Settled, StorageState.NotAccepted)]
    [InlineData(ValueState.Settled, StorageState.Sold)]
    public void ShouldReturnFalse(ValueState valueState, StorageState storageState)
    {
        // Arrange
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.ValueState, valueState)
            .With(product => product.StorageState, storageState)
            .Create();

        // Act
        bool result = product.WasPickedUp;

        // Assert
        result.Should().BeFalse();
    }
}
