using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class GetCommissionedPrice
    {
        [Theory]
        [InlineData(100.0, 100.0)]
        public void NoCommission(decimal price, decimal commisionedPrice)
        {
            var product = new Product
            {
                Price = price
            };
            var basar = new Basar();
            Assert.Equal(commisionedPrice, product.GetCommissionedPrice(basar));
        }
        [Theory]
        [InlineData(100.0, 90.0)]
        public void TenPercent(decimal price, decimal commisionedPrice)
        {
            var product = new Product
            {
                Price = price
            };
            var basar = new Basar
            {
                ProductCommission = 0.1m
            };
            Assert.Equal(commisionedPrice, product.GetCommissionedPrice(basar));
        }
    }
}
