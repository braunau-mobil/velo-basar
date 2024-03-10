namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class IsLost
{
    private readonly VeloFixture _fixture = new();
    
    [Theory]
    [InlineData(StorageState.Lost)]
    public void ShouldBeTrue(StorageState storageState)
    {
        //  Arrange
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.StorageState, storageState)
            .Create();

        //  Act
        bool result = product.IsLost;

        //  Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(StorageState.Available)]
    [InlineData(StorageState.Locked)]
    [InlineData(StorageState.NotAccepted)]
    [InlineData(StorageState.Sold)]
    public void ShouldBeFalse(StorageState storageState)
    {
        //  Arrange
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.StorageState, storageState)
            .Create();

        //  Act
        bool result = product.IsLost;

        //  Assert
        result.Should().BeFalse();
    }
}
