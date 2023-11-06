using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductsCount
    : TestBase<EmptySqliteDbFixture>
{
    [Fact]
    public void EmptyList_ReturnsZero()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        decimal result = Sut.GetSoldProductsCount(products);

        //  Assert
        result.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Fact]
    public void Products_ReturnsSumOfAcceptedProducts()
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<ProductEntity> soldProducts = fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Sold)
            .With(_ => _.Price, 1)
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.Sold);
        IEnumerable<ProductEntity> notSoldProducts = fixture.BuildProductEntity()
            .CreateMany();
        
        List<ProductEntity> products = new (soldProducts);
        products.AddRange(notSoldProducts);

        //  Act
        decimal result = Sut.GetSoldProductsCount(products);

        //  Assert
        result.Should().Be(soldProducts.Count());

        VerifyNoOtherCalls();
    }
}
