using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class GetManyAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task OrderShouldBeLikeParameter()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType, 666.66m).CreateMany(4).ToArray());
                var insertedProducts = acceptance.Products.GetProducts();

                var ids = new List<int>
                {
                    insertedProducts[3].Id,
                    insertedProducts[0].Id,
                    insertedProducts[2].Id,
                    insertedProducts[1].Id
                };

                var products = await ProductContext.GetManyAsync(ids);
                Assert.Equal(insertedProducts[3].Id, products[0].Id);
                Assert.Equal(insertedProducts[0].Id, products[1].Id);
                Assert.Equal(insertedProducts[2].Id, products[2].Id);
                Assert.Equal(insertedProducts[1].Id, products[3].Id);
            });
        }
    }
}
