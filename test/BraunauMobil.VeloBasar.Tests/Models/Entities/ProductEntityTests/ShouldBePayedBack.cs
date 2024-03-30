namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class ShouldBePayedBack
{
    [Theory]
    [InlineData(ValueState.NotSettled, StorageState.Available)]
    [InlineData(ValueState.Settled, StorageState.Available)]
    [InlineData(ValueState.NotSettled, StorageState.Locked)]
    [InlineData(ValueState.Settled, StorageState.Locked)]
    [InlineData(ValueState.Settled, StorageState.Lost)]
    [InlineData(ValueState.NotSettled, StorageState.NotAccepted)]
    [InlineData(ValueState.Settled, StorageState.NotAccepted)]
    [InlineData(ValueState.Settled, StorageState.Sold)]
    public void ShouldNot(ValueState valueState, StorageState storageState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = valueState
        };
        product.ShouldBePayedBack().Should().BeFalse();
    }
    [Theory]
    [InlineData(ValueState.NotSettled, StorageState.Lost)]
    [InlineData(ValueState.NotSettled, StorageState.Sold)]
    public void Should(ValueState valueState, StorageState storageState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = valueState
        };
        product.ShouldBePayedBack().Should().BeTrue();
    }
}
