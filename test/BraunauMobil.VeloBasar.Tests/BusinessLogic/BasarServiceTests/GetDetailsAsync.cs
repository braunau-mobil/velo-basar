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
        details.LockedProductsCount.Should().Be(4);
        details.LostProductsCount.Should().Be(6);
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
        details.SaleCount.Should().Be(41);
        details.SaleDistribution.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(5730.48M, "11:22", color)
        });
        details.SellerCount.Should().Be(28);
        details.SettlementPercentage.Should().Be(50);
        details.SoldProductsAmount.Should().Be(5730.48M);
        details.SoldProductsByAmount.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(836.27M, "City-Bike Herren", color),
            new ChartDataPoint(1051.40M, "Einrad", color),
            new ChartDataPoint(1209.03M, "Stahlross", color),
            new ChartDataPoint(619.86M, "Roller", color),
            new ChartDataPoint(495.18M, "Rennrad", color),
            new ChartDataPoint(270.08M, "Kinderrad", color),
            new ChartDataPoint(650.97M, "E-Bike", color),
            new ChartDataPoint(597.69M, "City-Bike Frauen", color)
        });
        details.SoldProductsByCount.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(9, "City-Bike Herren", color),
            new ChartDataPoint(11, "Einrad", color),
            new ChartDataPoint(12, "Stahlross", color),
            new ChartDataPoint(3, "Kinderrad", color),
            new ChartDataPoint(5, "Rennrad", color),
            new ChartDataPoint(7, "E-Bike", color),
            new ChartDataPoint(6, "City-Bike Frauen", color),
            new ChartDataPoint(6, "Roller", color),
        });
        details.SoldProductsCount.Should().Be(59);

        ColorProvider.Verify(_ => _.Primary, Times.AtLeastOnce());
        ColorProvider.VerifyGet(_ => _[It.IsAny<string>()], Times.AtLeastOnce());
        VerifyNoOtherCalls();
    }
}
