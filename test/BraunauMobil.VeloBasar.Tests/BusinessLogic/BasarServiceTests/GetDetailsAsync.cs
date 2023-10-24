using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public sealed class GetDetailsAsync
    : TestBase<SampleSqliteDbFixture>
{
    private readonly CultureInfo _initialCultureInfo = CultureInfo.CurrentCulture;
    
    public GetDetailsAsync()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-AT");
    }

    public override void Dispose()
    {
        base.Dispose();
        CultureInfo.CurrentCulture = _initialCultureInfo;
    }

    [Theory]
    [AutoData]
    public async Task ValuesShouldBeCorrect(Color color)
    {
        //  Arrange
        ColorProvider.Setup(_ => _.Primary)
            .Returns(color);
        ColorProvider.SetupGet(_ => _[It.IsAny<string>()])
            .Returns(color);

        //  Act
        BasarDetailsModel details = await Sut.GetDetailsAsync(1);

        //  Assert
        details.AcceptanceCount.Should().Be(52);
        details.AcceptedProductsAmount.Should().Be(12995.81M);
        details.AcceptedProductsByAmount.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(1598.13M, "City-Bike Herren", color),
            new ChartDataPoint(1860.51M, "Einrad", color),
            new ChartDataPoint(1937.27M, "Stahlross", color),
            new ChartDataPoint(817.89M, "Roller", color),
            new ChartDataPoint(2237.50M, "Rennrad", color),
            new ChartDataPoint(990.78M, "Kinderrad", color),
            new ChartDataPoint(1566.57M, "E-Bike", color),
            new ChartDataPoint(1987.16M, "City-Bike Frauen", color)
        });
        details.AcceptedProductsByCount.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(17, "City-Bike Herren", color),
            new ChartDataPoint(19, "Einrad", color),
            new ChartDataPoint(19, "Stahlross", color),
            new ChartDataPoint(8, "Roller", color),
            new ChartDataPoint(22, "Rennrad", color),
            new ChartDataPoint(10, "Kinderrad", color),
            new ChartDataPoint(16, "E-Bike", color),
            new ChartDataPoint(21, "City-Bike Frauen", color)
        });
        details.AcceptedProductsCount.Should().Be(132);
        details.LockedProductsCount.Should().Be(2);
        details.LostProductsCount.Should().Be(1);
        details.PriceDistribution.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(1, "€ 50,00", color),
            new ChartDataPoint(2, "€ 60,00", color),
            new ChartDataPoint(3, "€ 70,00", color),
            new ChartDataPoint(15, "€ 80,00", color),
            new ChartDataPoint(16, "€ 90,00", color),
            new ChartDataPoint(26, "€ 100,00", color),
            new ChartDataPoint(37, "€ 110,00", color),
            new ChartDataPoint(18, "€ 120,00", color),
            new ChartDataPoint(11, "€ 130,00", color)
        });
        details.SaleCount.Should().Be(33);
        details.SaleDistribution.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(7637.57M, "11:22", color)
        });
        details.SellerCount.Should().Be(28);
        details.SettlementPercentage.Should().Be(28);
        details.SoldProductsAmount.Should().Be(7637.57M);
        details.SoldProductsByAmount.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(869.23M, "City-Bike Herren", color),
            new ChartDataPoint(1305.09M, "Einrad", color),
            new ChartDataPoint(1314.29M, "Stahlross", color),
            new ChartDataPoint(312.46M, "Roller", color),
            new ChartDataPoint(1645.11M, "Rennrad", color),
            new ChartDataPoint(773.52M, "Kinderrad", color),
            new ChartDataPoint(648.36M, "E-Bike", color),
            new ChartDataPoint(769.51M, "City-Bike Frauen", color)
        });
        details.SoldProductsByCount.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(10, "City-Bike Herren", color),
            new ChartDataPoint(14, "Einrad", color),
            new ChartDataPoint(13, "Stahlross", color),
            new ChartDataPoint(3, "Roller", color),
            new ChartDataPoint(16, "Rennrad", color),
            new ChartDataPoint(7, "Kinderrad", color),
            new ChartDataPoint(7, "E-Bike", color),
            new ChartDataPoint(8, "City-Bike Frauen", color)
        });
        details.SoldProductsCount.Should().Be(78);

        ColorProvider.Verify(_ => _.Primary, Times.AtLeastOnce());
        ColorProvider.VerifyGet(_ => _[It.IsAny<string>()], Times.AtLeastOnce());
        VerifyNoOtherCalls();
    }
}
