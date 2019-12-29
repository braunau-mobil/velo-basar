using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class IsLocked
    {
        [Theory]
        [InlineData(StorageState.Locked)]
        public void Locked(StorageState storageState)
        {
            var product = new Product
            {
                StorageState = storageState
            };
            Assert.True(product.IsLocked());
        }
        [Theory]
        [InlineData(StorageState.Available)]
        [InlineData(StorageState.Gone)]
        [InlineData(StorageState.Sold)]
        public void NotLocked(StorageState storageState)
        {
            var product = new Product
            {
                StorageState = storageState
            };
            Assert.False(product.IsLocked());
        }
    }
}
