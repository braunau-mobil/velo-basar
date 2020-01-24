using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class AcceptProductsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task TwoProducts()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(2).ToList());

                var products = await ProductContext.GetProductsForSeller(basar, seller.Id).ToArrayAsync();
                Assert.Equal(2, products.Length);

                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.NotSettled, seller.ValueState);
            });
        }
    }
}