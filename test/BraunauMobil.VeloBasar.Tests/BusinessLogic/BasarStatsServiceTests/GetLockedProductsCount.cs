using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetLockedProductsCount
    : TestBase<EmptySqliteDbFixture>
{
    [Fact]
    public void EmptyList_ReturnsZero()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        decimal result = Sut.GetLockedProductsCount(products);

        //  Assert
        result.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Fact]
    public void Products_ReturnsSumOfAcceptedProducts()
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<ProductEntity> lockedProducts = fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Locked)
            .With(_ => _.Price, 1)
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.Locked);
        IEnumerable<ProductEntity> notSoldProducts = fixture.BuildProductEntity()
            .CreateMany();
        
        List<ProductEntity> products = new (lockedProducts);
        products.AddRange(notSoldProducts);

        //  Act
        decimal result = Sut.GetLockedProductsCount(products);

        //  Assert
        result.Should().Be(lockedProducts.Count());

        VerifyNoOtherCalls();
    }
}
