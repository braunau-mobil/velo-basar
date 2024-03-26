using BraunauMobil.VeloBasar.Configuration;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetPriceDistribution
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Fact]
    public void EmptyList_ShouldReturnEmpty()
    {
        // Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        // Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetPriceDistribution(products);

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void Products_ShouldReturnDistribution(Color primaryColor)
    {
        // Arrange
        ApplicationSettings.PriceDistributionRanges = [
            new PriceRange(50, null),
            new PriceRange(null, 50),
            new PriceRange(100, 200),
            new PriceRange(1000, 10000)
        ];
        A.CallTo(() => ColorProvider[A<string>.Ignored]).Returns(primaryColor);
        ProductEntity[] products =
        [
            CreateProduct(0),
            CreateProduct(0),
            CreateProduct(1),
            CreateProduct(5),
            CreateProduct(10),
            CreateProduct(10.01M),
            CreateProduct(11),
            CreateProduct(100),
            CreateProduct(1000),
            CreateProduct(9989.89M),
            CreateProduct(9999.99M),
        ];
        
        // Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetPriceDistribution(products);

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(4, "€ 50,00+", primaryColor),
            new ChartDataPoint(7, "-€ 50,00", primaryColor),
            new ChartDataPoint(1, "€ 100,00 - € 200,00", primaryColor),
            new ChartDataPoint(3, "€ 1.000,00 - € 10.000,00", primaryColor),
        });

        A.CallTo(() => ColorProvider[A<string>.Ignored]).MustHaveHappened(4, Times.Exactly);
    }

    private ProductEntity CreateProduct(decimal price)
        => _fixture.BuildProduct()
            .With(_ => _.Price, price)
            .Create();
}
