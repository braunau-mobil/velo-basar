namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class IsGone
{
    [Theory]
    [InlineData(StorageState.Lost)]
    public void Gone(StorageState storageState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState
        };
        Assert.True(product.IsLost());
    }

    [Theory]
    [InlineData(StorageState.Available)]
    [InlineData(StorageState.Locked)]
    [InlineData(StorageState.NotAccepted)]
    [InlineData(StorageState.Sold)]
    public void NotGone(StorageState storageState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState
        };
        Assert.False(product.IsLost());
    }
}
