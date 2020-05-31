using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class ShouldBePayedOut
    {
        [Theory]
        [InlineData(ValueState.NotSettled, StorageState.Available)]
        [InlineData(ValueState.Settled, StorageState.Available)]
        [InlineData(ValueState.NotSettled, StorageState.Locked)]
        [InlineData(ValueState.Settled, StorageState.Locked)]
        [InlineData(ValueState.NotSettled, StorageState.Gone)]
        [InlineData(ValueState.NotSettled, StorageState.Sold)]
        public void False(ValueState valueState, StorageState storageState)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = valueState
            };
            Assert.False(product.ShouldBePayedOut());
        }
        [Theory]
        [InlineData(ValueState.Settled, StorageState.Gone)]
        [InlineData(ValueState.Settled, StorageState.Sold)]
        public void True(ValueState valueState, StorageState storageState)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = valueState
            };
            Assert.True(product.ShouldBePayedOut());
        }
    }
}
