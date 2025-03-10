﻿using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductTypesWithCount
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Fact]
    public void NoProducts_ShouldReturnEmptyList()
    {
        // Arrange    
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        // Act
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetSoldProductTypesWithCount(products);

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void Products_ShouldFilterSoldAndGroupByTypeAndSumPrices(ProductTypeEntity typeA, ProductTypeEntity typeB, Color colorA, Color colorB)
    {
        //  Arrange
        typeA.Name = "A";
        typeB.Name = "B";
        A.CallTo(() => ColorProvider[typeA.Name]).Returns(colorA);
        A.CallTo(() => ColorProvider[typeB.Name]).Returns(colorB);
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
        IReadOnlyCollection<ChartDataPoint> result = Sut.GetSoldProductTypesWithCount(products);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(new []
            {
                new ChartDataPoint(2, typeA.Name, colorA),
                new ChartDataPoint(3, typeB.Name, colorB)
            });
        }

        A.CallTo(() => ColorProvider[typeA.Name]).MustHaveHappenedOnceExactly();
        A.CallTo(() => ColorProvider[typeB.Name]).MustHaveHappenedOnceExactly();
    }

    private ProductEntity CreateProduct(ProductTypeEntity type, StorageState storageState, decimal price)
        => _fixture.BuildProduct()
            .With(_ => _.Type, type)
            .With(_ => _.StorageState, storageState)
            .With(_ => _.Price, price)
            .Create();
}
