using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.Integration
{
    public class AcceptCheckoutSettleAccept : TestWithServicesAndDb
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

                
                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(2).ToList());
                var products = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                ModelAssert.ProductState(StorageState.Available, ValueState.NotSettled, products);
                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.NotSettled, seller.ValueState);

                
                await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Take(1).Select(pt => pt.ProductId).ToList());
                products = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                ModelAssert.ProductState(StorageState.Available, ValueState.NotSettled, products[0]);
                ModelAssert.ProductState(StorageState.Sold, ValueState.NotSettled, products[1]);
                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.NotSettled, seller.ValueState);

                
                var settlement = await TransactionContext.SettleSellerAsync(basar, seller.Id);
                Assert.Equal(2, settlement.Products.Count);
                products = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                ModelAssert.ProductState(StorageState.Available, ValueState.Settled, products[0]);
                ModelAssert.ProductState(StorageState.Sold, ValueState.Settled, products[1]);
                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.Settled, seller.ValueState);


                var acceptance2 = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(2).ToList());
                products = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                ModelAssert.ProductState(StorageState.Available, ValueState.Settled, products[0]);
                ModelAssert.ProductState(StorageState.Sold, ValueState.Settled, products[1]);
                ModelAssert.ProductState(StorageState.Available, ValueState.NotSettled, products[2]);
                ModelAssert.ProductState(StorageState.Available, ValueState.NotSettled, products[3]);
                seller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.NotSettled, seller.ValueState);
            });
        }
    }
}
