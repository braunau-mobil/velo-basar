using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class CancelProductsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task OneFromTwo()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));

                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(2).ToList());
                var sale = await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Select(pt => pt.ProductId).ToList());

                await TransactionContext.CancelProductsAsync(basar, sale.Id, new List<int> { 1 });

                var updatedSale = await TransactionContext.GetAsync(sale.Id);
                Assert.Equal(1, updatedSale.Products.Count);
                var updatedProducts = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                Assert.Equal(StorageState.Available, updatedProducts[0].StorageState);
                Assert.Equal(ValueState.NotSettled, updatedProducts[0].ValueState);
                Assert.Equal(StorageState.Sold, updatedProducts[1].StorageState);
                Assert.Equal(ValueState.NotSettled, updatedProducts[1].ValueState);
            });
        }
        [Fact]
        public async Task All()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Create<Basar>());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));

                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());

                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, fixture.BuildProduct(brand, productType).CreateMany(2).ToList());
                var sale = await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Select(pt => pt.ProductId).ToList());

                var cancellation = await TransactionContext.CancelProductsAsync(basar, sale.Id, new List<int> { 1, 2 });

                var updatedSale = await TransactionContext.GetAsync(sale.Id);
                Assert.Equal(null, updatedSale);
                var updatedProducts = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                Assert.Equal(StorageState.Available, updatedProducts[0].StorageState);
                Assert.Equal(ValueState.NotSettled, updatedProducts[0].ValueState);
                Assert.Equal(StorageState.Available, updatedProducts[1].StorageState);
                Assert.Equal(ValueState.NotSettled, updatedProducts[1].ValueState);

                var document = await FileStoreContext.GetAsync(sale.DocumentId.Value);
                Assert.Null(document);
            });
        }
    }
}
