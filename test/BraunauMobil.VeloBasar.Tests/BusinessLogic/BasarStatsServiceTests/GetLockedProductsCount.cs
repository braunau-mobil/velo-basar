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
    }

    [Fact]
    public void Products_ReturnsSumOfAcceptedProducts()
    {
        //  Arrange
        VeloFixture fixture = new();
        IEnumerable<ProductEntity> lockedProducts = fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.Locked)
            .With(_ => _.Price, 1)
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.Locked);
        IEnumerable<ProductEntity> notSoldProducts = fixture.BuildProduct()
            .CreateMany();
        
        List<ProductEntity> products = new (lockedProducts);
        products.AddRange(notSoldProducts);

        //  Act
        decimal result = Sut.GetLockedProductsCount(products);

        //  Assert
        result.Should().Be(lockedProducts.Count());
    }
}
