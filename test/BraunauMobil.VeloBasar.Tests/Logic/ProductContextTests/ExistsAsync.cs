using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class ExistsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task Test()
        {
            await RunOnInitializedDb(async () =>
            {
                var basar = await BasarContext.CreateAsync(new Basar());

                var fixture = new Fixture();
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.CreateMany<Product>(1).ToList());

                Assert.True(await ProductContext.ExistsAsync(1));
                Assert.False(await ProductContext.ExistsAsync(2));
            });
        }
    }
}
