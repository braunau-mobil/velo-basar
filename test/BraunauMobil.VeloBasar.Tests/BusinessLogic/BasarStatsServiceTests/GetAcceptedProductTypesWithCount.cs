using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetAcceptedProductTypesWithCount
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void NoProducts_ShouldReturnEmptyList()
    {
        // Arrange    
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        // Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetAcceptedProductTypesWithCount(products);

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public void Products_ShouldGroupByTypeAndSumPrices(ProductTypeEntity typeA, ProductTypeEntity typeB, Color colorA, Color colorB)
    {
        //  Arrange
        A.CallTo(() => ColorProvider[typeA.Name]).Returns(colorA);
        A.CallTo(() => ColorProvider[typeB.Name]).Returns(colorB);
        ProductEntity[] products = new[]
        {
            CreateProduct(typeA),
            CreateProduct(typeB),
            CreateProduct(typeA),
            CreateProduct(typeB),
            CreateProduct(typeB),
        };

        //  Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetAcceptedProductTypesWithCount(products);

        //  Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new []
        {
            new ChartDataPoint(2, typeA.Name, colorA),
            new ChartDataPoint(3, typeB.Name, colorB)
        });

        A.CallTo(() => ColorProvider[typeA.Name]).MustHaveHappenedOnceExactly();
        A.CallTo(() => ColorProvider[typeB.Name]).MustHaveHappenedOnceExactly();
    }

    private ProductEntity CreateProduct(ProductTypeEntity type)
        => _fixture.BuildProductEntity()
            .With(_ => _.Type, type)
            .Create();
}
