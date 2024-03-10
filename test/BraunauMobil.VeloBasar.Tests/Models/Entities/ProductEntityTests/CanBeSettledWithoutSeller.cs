namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class CanBeSettledWithoutSeller
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [InlineData(StorageState.Lost, false)]
    [InlineData(StorageState.Sold, false)]
    [InlineData(StorageState.Available, true)]
    [InlineData(StorageState.Locked, true)]
    [InlineData(StorageState.Lost, true)]
    [InlineData(StorageState.Sold, true)]
    public void ShouldReturnTrue(StorageState storageState, bool donateIfNotSold)
    {
        //  Arrange
        ProductEntity sut = _fixture.BuildProduct()
            .With(_ => _.StorageState, storageState)
            .With(_ => _.DonateIfNotSold, donateIfNotSold)
            .Create();

        //  Act
        bool result = sut.CanBeSettledWithoutSeller;

        //  Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(StorageState.NotAccepted, false)]
    [InlineData(StorageState.Available, false)]
    [InlineData(StorageState.Locked, false)]
    [InlineData(StorageState.NotAccepted, true)]
    public void ShouldReturnFalse(StorageState storageState, bool donateIfNotSold)
    {
        //  Arrange
        ProductEntity sut = _fixture.BuildProduct()
            .With(_ => _.StorageState, storageState)
            .With(_ => _.DonateIfNotSold, donateIfNotSold)
            .Create();

        //  Act
        bool result = sut.CanBeSettledWithoutSeller;

        //  Assert
        result.Should().BeFalse();
    }
}
