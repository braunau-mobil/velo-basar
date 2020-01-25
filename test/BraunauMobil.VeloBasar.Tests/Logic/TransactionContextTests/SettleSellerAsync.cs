using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class SettleSellerAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task OneSoldOneNotSold()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(2).ToList());

                await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Take(1).Select(pt => pt.ProductId).ToList());

                var settlement = await TransactionContext.SettleSellerAsync(basar, seller.Id);
                Assert.Equal(2, settlement.Products.Count);

                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.Settled, seller.ValueState);
            });
        }
        [Fact]
        public async Task OneSoldOneNotSoldOneLockedOneGone()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var seller = await SellerContext.CreateAsync(fixture.Create<Seller>());

                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(4).ToList());

                await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Where(p => p.ProductId == 1).Select(pt => pt.ProductId).ToList());
                await TransactionContext.DoTransactionAsync(basar, TransactionType.Lock, "Test", 2);
                await TransactionContext.DoTransactionAsync(basar, TransactionType.MarkAsGone, "Test", 3);

                var settlement = await TransactionContext.SettleSellerAsync(basar, seller.Id);
                Assert.Equal(4, settlement.Products.Count);

                var products = await ProductContext.GetProductsForSeller(basar, seller.Id).OrderBy(p => p.Id).ToArrayAsync();
                ModelAssert.ProductState(StorageState.Sold, ValueState.Settled, products[0]);
                ModelAssert.ProductState(StorageState.Locked, ValueState.Settled, products[1]);
                ModelAssert.ProductState(StorageState.Gone, ValueState.Settled, products[2]);
                ModelAssert.ProductState(StorageState.Available, ValueState.Settled, products[3]);

                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.Settled, seller.ValueState);
            });
        }
    }
}
