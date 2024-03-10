namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class IsLocked
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [InlineData(StorageState.Locked)]
    public void ShouldBeTrue(StorageState storageState)
    {
        //  Arrange
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.StorageState, storageState)
            .Create();

        //  Act
        bool result = product.IsLocked;

        //  Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(StorageState.Available)]
    [InlineData(StorageState.Lost)]
    [InlineData(StorageState.NotAccepted)]
    [InlineData(StorageState.Sold)]
    public void NotLocked(StorageState storageState)
    {
        //  Arrange
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.StorageState, storageState)
            .Create();

        //  Act
        bool result = product.IsLocked;

        //  Assert
        result.Should().BeFalse();
    }
}
