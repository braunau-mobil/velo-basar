using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests
{
    public class SumPrice
    {
        [Fact]
        public void Test()
        {
            var products = new[]
            {
                new Product
                {
                    Price = 123.12m,
                    StorageState = StorageState.Sold
                },
                new Product
                {
                    Price = 99.99m,
                    StorageState = StorageState.Sold
                },
                new Product
                {
                    Price = 100.0m,
                    StorageState = StorageState.Available
                }
            };
            Assert.Equal(323.11m, products.SumPrice());
        }
    }
}
