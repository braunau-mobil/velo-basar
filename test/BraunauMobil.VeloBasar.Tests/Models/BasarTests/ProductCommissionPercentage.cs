using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.BasarTests
{
    public class ProductCommissionPercentage
    {
        [Theory]
        [InlineData(0.0, 0)]
        [InlineData(0.2, 20)]
        [InlineData(1.0, 100)]
        public void Get(decimal productCommision, int exptectedPercentage)
        {
            var basar = new Basar
            {
                ProductCommission = productCommision
            };
            Assert.Equal(exptectedPercentage, basar.ProductCommissionPercentage);
        }
        [Theory]
        [InlineData(0, 0.0)]
        [InlineData(20, 0.2)]
        [InlineData(100, 1.0)]
        public void Set(int valueToSet, decimal exptectedCommision)
        {
            var basar = new Basar
            {
                ProductCommissionPercentage = valueToSet
            };
            Assert.Equal(exptectedCommision, basar.ProductCommission);
        }
    }
}
