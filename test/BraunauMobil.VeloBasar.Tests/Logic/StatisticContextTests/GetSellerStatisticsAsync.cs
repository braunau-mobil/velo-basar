using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.StatisticContextTests
{
    public class GetSellerStatisticsAsync : TestWithServicesAndDb
    {
        [Fact]
        public async Task Test()
        {
            await RunOnInitializedDb(async () =>
            {
                var fixture = new Fixture();

                var basar = await BasarContext.CreateAsync(fixture.Build<Basar>().With(b => b.ProductCommission, 0.1m).Create());
                var country = await CountryContext.CreateAsync(fixture.Create<Country>());
                var seller = await SellerContext.CreateAsync(fixture.CreateSeller(country));

                var productType = await ProductTypeContext.CreateAsync(fixture.Create<ProductType>());
                var brand = await BrandContext.CreateAsync(fixture.Create<Brand>());
                var products = fixture.CreateManyProducts(5, brand, productType, 100.0m).ToList();
                var acceptance = await TransactionContext.AcceptProductsAsync(basar, seller.Id, products);

                await TransactionContext.CheckoutProductsAsync(basar, acceptance.Products.Select(p => p.ProductId).ToArray());
                await TransactionContext.SettleSellerAsync(basar, seller.Id);

                var statistic = await StatisticContext.GetSellerStatisticsAsync(basar, seller.Id);
                Assert.Equal(5, statistic.AceptedProductCount);
                Assert.Equal(0, statistic.NotSoldProductCount);
                Assert.Equal(0, statistic.PickedUpProductCount);
                Assert.Equal(450.0m, statistic.SettlementAmout);
                Assert.Equal(5, statistic.SoldProductCount);
            });
        }
    }
}
