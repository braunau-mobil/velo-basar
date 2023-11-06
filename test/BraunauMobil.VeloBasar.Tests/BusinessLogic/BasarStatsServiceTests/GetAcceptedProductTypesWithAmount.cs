using System.Collections.Generic;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetAcceptedProductTypesWithAmount
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void NoProducts_ShouldReturnEmptyList()
    {
        // Arrange    
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        // Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetAcceptedProductTypesWithAmount(products);

        // Assert
        result.Should().BeEmpty();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void Products_ShouldGroupByTypeAndSumPrices(ProductTypeEntity typeA, ProductTypeEntity typeB, Color colorA, Color colorB)
    {
        //  Arrange
        ColorProvider.SetupGet(_ => _[typeA.Name])
            .Returns(colorA);
        ColorProvider.SetupGet(_ => _[typeB.Name])
            .Returns(colorB);
        ProductEntity[] products = new[]
        {
            CreateProduct(typeA, 10),
            CreateProduct(typeB, 20),
            CreateProduct(typeA, 15),
            CreateProduct(typeB, 5),
            CreateProduct(typeB, 100),
        };

        //  Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetAcceptedProductTypesWithAmount(products);

        //  Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new []
        {
            new ChartDataPoint(25, typeA.Name, colorA),
            new ChartDataPoint(125, typeB.Name, colorB)
        });

        ColorProvider.VerifyGet(_ => _[typeA.Name], Times.Once());
        ColorProvider.VerifyGet(_ => _[typeB.Name], Times.Once());
        VerifyNoOtherCalls();
    }

    private ProductEntity CreateProduct(ProductTypeEntity type, decimal price)
        => _fixture.BuildProductEntity()
            .With(_ => _.Type, type)
            .With(_ => _.Price, price)
            .Create();
}
