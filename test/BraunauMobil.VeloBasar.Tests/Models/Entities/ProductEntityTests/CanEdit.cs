namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class CanEdit
{
    [Theory]
    [InlineData(StorageState.Available, ValueState.NotSettled)]
    [InlineData(StorageState.NotAccepted, ValueState.NotSettled)]
    public void Allow(StorageState storageState, ValueState valueState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = valueState
        };
        Assert.True(product.CanEdit);
    }

    [Theory]
    [InlineData(StorageState.Lost, ValueState.NotSettled)]
    [InlineData(StorageState.Locked, ValueState.NotSettled)]
    [InlineData(StorageState.Sold, ValueState.NotSettled)]
    [InlineData(StorageState.Available, ValueState.Settled)]
    [InlineData(StorageState.Lost, ValueState.Settled)]
    [InlineData(StorageState.Locked, ValueState.Settled)]
    [InlineData(StorageState.Sold, ValueState.Settled)]

    public void Disallow(StorageState storageState, ValueState valueState)
    {
        ProductEntity product = new ()
        {
            StorageState = storageState,
            ValueState = valueState
        };
        Assert.False(product.CanEdit);
    }
}
