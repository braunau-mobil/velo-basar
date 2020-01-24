using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class DoTransactionAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task Lock()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(1).ToList());

                var lockTx = await TransactionContext.DoTransactionAsync(basar, TransactionType.Lock, "These are my notes", 1);
                Assert.Equal(basar, lockTx.Basar);
                Assert.Equal(basar.Id, lockTx.BasarId);
                Assert.Null(lockTx.DocumentId);
                Assert.Equal(2, lockTx.Id);
                Assert.Equal("These are my notes", lockTx.Notes);
                Assert.Equal(1, lockTx.Number);
                Assert.Null(lockTx.Seller);
                Assert.Null(lockTx.SellerId);
                Assert.Equal(TransactionType.Lock, lockTx.Type);
            });
        }
    }
}
