using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests
{
    public class GetDetailsAsync
        : SampleDatabaseTestBase
    {
        [Fact]
        public async Task ValuesShouldBeCorrect()
        {
            //  Arrange
            IBasarService sut = ServiceScope.ServiceProvider.GetRequiredService<IBasarService>();

            //  Act
            BasarDetailsModel details = await sut.GetDetailsAsync(1);

            //  Assert
            details.AcceptanceCount.Should().Be(52);
            details.AcceptedProductsAmount.Should().Be(12995.81M);
            details.AcceptedProductsByAmount.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(1598.13M, "City-Bike Herren", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1860.51M, "Einrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1937.27M, "Stahlross", OnlyBlackColorProvider.Black),
                new ChartDataPoint(817.89M, "Roller", OnlyBlackColorProvider.Black),
                new ChartDataPoint(2237.50M, "Rennrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(990.78M, "Kinderrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1566.57M, "E-Bike", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1987.16M, "City-Bike Frauen", OnlyBlackColorProvider.Black)
            });
            details.AcceptedProductsByCount.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(17, "City-Bike Herren", OnlyBlackColorProvider.Black),
                new ChartDataPoint(19, "Einrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(19, "Stahlross", OnlyBlackColorProvider.Black),
                new ChartDataPoint(8, "Roller", OnlyBlackColorProvider.Black),
                new ChartDataPoint(22, "Rennrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(10, "Kinderrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(16, "E-Bike", OnlyBlackColorProvider.Black),
                new ChartDataPoint(21, "City-Bike Frauen", OnlyBlackColorProvider.Black)
            });
            details.AcceptedProductsCount.Should().Be(132);
            details.LockedProductsCount.Should().Be(2);
            details.LostProductsCount.Should().Be(1);
            details.PriceDistribution.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(1, "€ 50,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(2, "€ 60,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(3, "€ 70,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(15, "€ 80,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(16, "€ 90,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(26, "€ 100,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(37, "€ 110,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(18, "€ 120,00", OnlyBlackColorProvider.Black),
                new ChartDataPoint(11, "€ 130,00", OnlyBlackColorProvider.Black)
            });
            details.SaleCount.Should().Be(33);
            details.SaleDistribution.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(7637.57M, "00:00", OnlyBlackColorProvider.Black)
            });
            details.SellerCount.Should().Be(28);
            details.SettlementPercentage.Should().Be(28);
            details.SoldProductsAmount.Should().Be(7637.57M);
            details.SoldProductsByAmount.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(869.23M, "City-Bike Herren", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1305.09M, "Einrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1314.29M, "Stahlross", OnlyBlackColorProvider.Black),
                new ChartDataPoint(312.46M, "Roller", OnlyBlackColorProvider.Black),
                new ChartDataPoint(1645.11M, "Rennrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(773.52M, "Kinderrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(648.36M, "E-Bike", OnlyBlackColorProvider.Black),
                new ChartDataPoint(769.51M, "City-Bike Frauen", OnlyBlackColorProvider.Black)
            });
            details.SoldProductsByCount.Should().BeEquivalentTo(new[]
            {
                new ChartDataPoint(10, "City-Bike Herren", OnlyBlackColorProvider.Black),
                new ChartDataPoint(14, "Einrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(13, "Stahlross", OnlyBlackColorProvider.Black),
                new ChartDataPoint(3, "Roller", OnlyBlackColorProvider.Black),
                new ChartDataPoint(16, "Rennrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(7, "Kinderrad", OnlyBlackColorProvider.Black),
                new ChartDataPoint(7, "E-Bike", OnlyBlackColorProvider.Black),
                new ChartDataPoint(8, "City-Bike Frauen", OnlyBlackColorProvider.Black)
            });
            details.SoldProductsCount.Should().Be(78);
        }
    }
}
