using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class GetLatestAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task Test()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());
                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());


                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(1).ToList());
                await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Take(1).Select(pt => pt.ProductId).ToList());
                var settlement = await TransactionContext.SettleSellerAsync(basar, seller.Id);

                var latest = await TransactionContext.GetLatestAsync(basar, 1);
                Assert.Equal(settlement, latest);
                Assert.Equal(TransactionType.Settlement, latest.Type);
            });
        }
    }
}
