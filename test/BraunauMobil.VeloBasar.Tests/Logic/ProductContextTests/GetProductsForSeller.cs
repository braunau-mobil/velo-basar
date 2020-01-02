using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.ProductContextTests
{
    public class GetProductsForSeller : TestWithServicesAndDb
    {
        [Fact]
        public async Task MultipleAcceptances()
        {
            await RunOnInitializedDb(async () =>
            {
                var basar = await BasarContext.CreateAsync(new Basar());

                var fixture = new Fixture();
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var firstAcceptance = TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.CreateMany<Product>(1).ToList());
                var secondAceptance = TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.CreateMany<Product>(2).ToList());

                var products = await ProductContext.GetProductsForSeller(basar, seller.Id).ToArrayAsync();
                Assert.Equal(3, products.Length);
            });
        }
    }
}
