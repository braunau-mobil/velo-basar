using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class InsertProductsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task One()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));
                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var products = fixture.BuildProduct(brand, productType, 666).CreateMany(1).ToList();

                var insertedProducts = await ProductContext.InsertProductsAsync(basar, seller, products);

                var first = insertedProducts[0];
                Assert.Equal(basar.Id, first.BasarId);
                Assert.Equal(seller.Id, first.SellerId);
            });
        }
    }
}
