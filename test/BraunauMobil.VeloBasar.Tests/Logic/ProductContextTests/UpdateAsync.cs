using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class UpdateAsync : TestWithServicesAndDb
    {
        [Fact]
        public async void BasarIsNull()
        {
            await RunOnInitializedDb(async db =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));
                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var products = fixture.BuildProduct(brand, productType, 666).CreateMany(1).ToList();

                var insertedProducts = await ProductContext.InsertProductsAsync(basar, seller, products);

                var product = insertedProducts[0];

                product.Basar = null;
                product.Price = 666m;

                await ProductContext.UpdateAsync(product);

                product = await ProductContext.GetAsync(product.Id);
                Assert.NotNull(product.Basar);
                Assert.Equal(666m, product.Price);
            });
        }
    }
}
