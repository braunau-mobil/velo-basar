namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class IsLocked
{
    [Theory]
    [InlineData(StorageState.Locked)]
    public void Locked(StorageState storageState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState
        };
        Assert.True(product.IsLocked());
    }

    [Theory]
    [InlineData(StorageState.Available)]
    [InlineData(StorageState.Lost)]
    [InlineData(StorageState.NotAccepted)]
    [InlineData(StorageState.Sold)]
    public void NotLocked(StorageState storageState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState
        };
        Assert.False(product.IsLocked());
    }
}
