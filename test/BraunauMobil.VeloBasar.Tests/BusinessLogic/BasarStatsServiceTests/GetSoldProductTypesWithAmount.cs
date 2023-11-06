using System.Collections.Generic;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductTypesWithAmount
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void NoProducts_ShouldReturnEmptyList()
    {
        // Arrange    
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        // Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetSoldProductTypesWithAmount(products);

        // Assert
        result.Should().BeEmpty();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void Products_ShouldFilterSoldAndGroupByTypeAndSumPrices(ProductTypeEntity typeA, ProductTypeEntity typeB, Color colorA, Color colorB)
    {
        //  Arrange
        ColorProvider.SetupGet(_ => _[typeA.Name])
            .Returns(colorA);
        ColorProvider.SetupGet(_ => _[typeB.Name])
            .Returns(colorB);
        ProductEntity[] products = new[]
        {
            CreateProduct(typeA, StorageState.Sold, 15),
            CreateProduct(typeA, StorageState.Locked, 22),
            CreateProduct(typeB, StorageState.Sold, 20),
            CreateProduct(typeA, StorageState.Sold, 15),
            CreateProduct(typeB, StorageState.Sold, 5),
            CreateProduct(typeB, StorageState.Available, 55),
            CreateProduct(typeB, StorageState.Sold, 100),
        };

        //  Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetSoldProductTypesWithAmount(products);

        //  Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new []
        {
            new ChartDataPoint(30, typeA.Name, colorA),
            new ChartDataPoint(125, typeB.Name, colorB)
        });

        ColorProvider.VerifyGet(_ => _[typeA.Name], Times.Once());
        ColorProvider.VerifyGet(_ => _[typeB.Name], Times.Once());
        VerifyNoOtherCalls();
    }

    private ProductEntity CreateProduct(ProductTypeEntity type, StorageState storageState, decimal price)
        => _fixture.BuildProductEntity()
            .With(_ => _.Type, type)
            .With(_ => _.StorageState, storageState)
            .With(_ => _.Price, price)
            .Create();
}
