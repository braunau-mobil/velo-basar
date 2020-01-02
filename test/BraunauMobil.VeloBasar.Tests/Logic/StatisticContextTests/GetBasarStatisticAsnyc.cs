using System;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Logic.StatisticContextTests
{
    public class GetBasarStatisticAsnyc : TestWithServicesAndDb
    {
        [Fact]
        public async Task EmptyBasar()
        {
            await RunOnInitializedDb(
                async () =>
                {
                    var basar = await BasarContext.CreateAsync(new VeloBasar.Models.Basar
                    {
                        Date = new DateTime(2063, 04, 05),
                        Location = "Hopfenhausen",
                        Name = "Testbasar",
                        ProductCommission = 0.1m
                    });

                    var basarStatistic = await StatisticContext.GetBasarStatisticAsnyc(basar.Id);

                    Assert.Equal(0.0m, basarStatistic.AcceptedProductsAmount);
                    Assert.Empty(basarStatistic.AcceptedProductsByAmount);
                    Assert.Empty(basarStatistic.AcceptedProductsByCount);
                    Assert.Equal(0, basarStatistic.AcceptedProductsCount);
                    Assert.Equal(0, basarStatistic.GoneProductsCount);
                    Assert.Equal(0, basarStatistic.LockedProductsCount);
                    Assert.Empty(basarStatistic.PriceDistribution);
                    Assert.Equal(0, basarStatistic.SellerCount);
                    Assert.Equal(0.0m, basarStatistic.SoldProductsAmount);
                    Assert.Empty(basarStatistic.SoldProductsByAmount);
                    Assert.Empty(basarStatistic.SoldProductsByCount);
                    Assert.Equal(0.0, basarStatistic.SoldProductsCount);
                });
        }
    }
}
