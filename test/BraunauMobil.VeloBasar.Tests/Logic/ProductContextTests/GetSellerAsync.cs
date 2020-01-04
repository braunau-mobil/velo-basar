using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class GetSellerAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task Exists()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));
                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(1).ToList());

                var retrievedSeller = await ProductContext.GetSellerAsync(basar, acceptance.Products.First().Product);
                Assert.Equal(seller.Id, retrievedSeller.Id);
            });
        }
    }
}
