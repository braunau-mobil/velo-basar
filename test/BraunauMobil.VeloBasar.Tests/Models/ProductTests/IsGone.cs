using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class IsGone
    {
        [Theory]
        [InlineData(StorageState.Gone)]
        public void Gone(StorageState storageState)
        {
            var product = new Product
            {
                StorageState = storageState
            };
            Assert.True(product.IsGone());
        }
        [Theory]
        [InlineData(StorageState.Available)]
        [InlineData(StorageState.Locked)]
        [InlineData(StorageState.Sold)]
        public void NotGone(StorageState storageState)
        {
            var product = new Product
            {
                StorageState = storageState
            };
            Assert.False(product.IsGone());
        }
    }
}
