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
        A.CallTo(() => ColorProvider.Primary).Returns(primaryColor);
        ProductEntity[] products = new[]
        {
            CreateProduct(0),
            CreateProduct(0),
            CreateProduct(1),
            CreateProduct(5),
            CreateProduct(10),
            CreateProduct(10.01M),
            CreateProduct(11),
            CreateProduct(100),
            CreateProduct(1000),
        };
        
        // Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetPriceDistribution(products);

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(5, "€ 10,00", primaryColor),
            new ChartDataPoint(2, "€ 20,00", primaryColor),
            new ChartDataPoint(1, "€ 100,00", primaryColor),
            new ChartDataPoint(1, "€ 1.000,00", primaryColor),
        });

        A.CallTo(() => ColorProvider.Primary).MustHaveHappened(4, Times.Exactly);
    }

    [Theory]
    [VeloAutoData]
    public void ProductsWithPricesLowerThan10_ShouldReturnDistribution(Color primaryColor)
    {
        // Arrange
        A.CallTo(() => ColorProvider.Primary).Returns(primaryColor);
        ProductEntity[] products = new[]
        {
            CreateProduct(0),
            CreateProduct(0),
            CreateProduct(1),
            CreateProduct(5)
        };
        
        // Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetPriceDistribution(products);

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new ChartDataPoint(4, "€ 5,00", primaryColor),
        });

        A.CallTo(() => ColorProvider.Primary).MustHaveHappenedOnceExactly();
    }

    private ProductEntity CreateProduct(decimal price)
        => _fixture.BuildProduct()
            .With(_ => _.Price, price)
            .Create();
}
