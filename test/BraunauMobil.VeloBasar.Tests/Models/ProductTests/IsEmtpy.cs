using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class IsEmtpy
    {
        [Fact]
        public void Empty()
        {
            var product = new Product();
            Assert.True(product.IsEmtpy());
        }
        [Fact]
        public void ColorSet()
        {
            var product = new Product
            {
                Color = "Test"
            };
            Assert.False(product.IsEmtpy());
        }
        [Fact]
        public void DescriptionSet()
        {
            var product = new Product
            {
                Description = "Test"
            };
            Assert.False(product.IsEmtpy());
        }
        [Fact]
        public void TireSizeSet()
        {
            var product = new Product
            {
                TireSize = "Test"
            };
            Assert.False(product.IsEmtpy());
        }
        [Fact]
        public void BrandSet()
        {
            var product = new Product
            {
                Brand = new Brand()
            };
            Assert.False(product.IsEmtpy());
        }
        [Fact]
        public void TypeSet()
        {
            var product = new Product
            {
                Type = new ProductType()
            };
            Assert.False(product.IsEmtpy());
        }
    }
}
