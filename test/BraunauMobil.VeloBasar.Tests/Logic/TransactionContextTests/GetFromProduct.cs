using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class GetFromProduct : TestWithServicesAndDb
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

                await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Select(pt => pt.ProductId).ToList());
                var settlement = await TransactionContext.SettleSellerAsync(basar, seller.Id);

                var transactions = await TransactionContext.GetFromProduct(basar, 1).ToArrayAsync();
                Assert.Equal(3, transactions.Length);
                Assert.Equal(TransactionType.Acceptance, transactions[0].Type);
                Assert.Equal(TransactionType.Sale, transactions[1].Type);
                Assert.Equal(TransactionType.Settlement, transactions[2].Type);
            });
        }
    }
}
