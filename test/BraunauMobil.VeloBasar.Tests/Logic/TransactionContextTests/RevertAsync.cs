using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.TransactionContextTests
{
    public class RevertAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task Settlement()
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

                var settlement = await TransactionContext.SettleSellerAsync(basar, seller.Id);
                var documentId = settlement.DocumentId.Value;

                await TransactionContext.RevertAsync(settlement);

                var updatedProducts = await ProductContext.GetProductsForBasar(basar).ToArrayAsync();
                foreach (var product in updatedProducts)
                {
                    Assert.Equal(ValueState.NotSettled, product.ValueState);
                }
                var updatedSeller = await SellerContext.GetAsync(seller.Id);
                Assert.Equal(ValueState.NotSettled, updatedSeller.ValueState);

                var document = await FileStoreContext.GetAsync(documentId);
                Assert.Null(document);
            });
        }
    }
}
