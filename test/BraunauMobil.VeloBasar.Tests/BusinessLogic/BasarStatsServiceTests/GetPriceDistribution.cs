using System.Collections.Generic;
using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetPriceDistribution
    : TestBase<EmptySqliteDbFixture>
{
    private readonly CultureInfo _initialCultureInfo = CultureInfo.CurrentCulture;
    private readonly Fixture _fixture = new();

    public GetPriceDistribution()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-AT");
    }

    public override void Dispose()
    {
        CultureInfo.CurrentCulture = _initialCultureInfo;
        base.Dispose();
    }

    [Fact]
    public void EmptyList_ShouldReturnEmpty()
    {
        // Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        // Act
        IReadOnlyList<ChartDataPoint> result = Sut.GetPriceDistribution(products);

        // Assert
        result.Should().BeEmpty();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void Products_ShouldReturnDistribution(Color primaryColor)
    {
        // Arrange
        ColorProvider.Setup(_ => _.Primary)
            .Returns(primaryColor);
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

        ColorProvider.Verify(_ => _.Primary, Times.Exactly(4));
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void ProductsWithPricesLowerThan10_ShouldReturnDistribution(Color primaryColor)
    {
        // Arrange
        ColorProvider.Setup(_ => _.Primary)
            .Returns(primaryColor);
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

        ColorProvider.Verify(_ => _.Primary, Times.Exactly(1));
        VerifyNoOtherCalls();
    }

    private ProductEntity CreateProduct(decimal price)
        => _fixture.Build<ProductEntity>()
            .With(_ => _.Price, price)
            .Create();
}
