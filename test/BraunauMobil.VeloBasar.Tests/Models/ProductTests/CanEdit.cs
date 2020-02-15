using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class CanEdit
    {
        [Theory]
        [InlineData(StorageState.Available, ValueState.NotSettled)]
        public void Allow(StorageState storageState, ValueState valueState)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = valueState
            };
            Assert.True(product.CanEdit());
        }
        [Theory]
        [InlineData(StorageState.Gone, ValueState.NotSettled)]
        [InlineData(StorageState.Locked, ValueState.NotSettled)]
        [InlineData(StorageState.Sold, ValueState.NotSettled)]
        [InlineData(StorageState.Available, ValueState.Settled)]
        [InlineData(StorageState.Gone, ValueState.Settled)]
        [InlineData(StorageState.Locked, ValueState.Settled)]
        [InlineData(StorageState.Sold, ValueState.Settled)]

        public void Disallow(StorageState storageState, ValueState valueState)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = valueState
            };
            Assert.False(product.CanEdit());
        }
    }
}
