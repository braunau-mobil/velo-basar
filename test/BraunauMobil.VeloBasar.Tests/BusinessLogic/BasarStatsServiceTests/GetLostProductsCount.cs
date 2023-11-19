namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetLostProductsCount
    : TestBase<EmptySqliteDbFixture>
{
    [Fact]
    public void EmptyList_ReturnsZero()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        decimal result = Sut.GetLostProductsCount(products);

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
            .With(_ => _.StorageState, StorageState.Lost)
            .With(_ => _.Price, 1)
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.Lost);
        IEnumerable<ProductEntity> notSoldProducts = fixture.BuildProductEntity()
            .CreateMany();
        
        List<ProductEntity> products = new (lockedProducts);
        products.AddRange(notSoldProducts);

        //  Act
        decimal result = Sut.GetLostProductsCount(products);

        //  Assert
        result.Should().Be(lockedProducts.Count());

        VerifyNoOtherCalls();
    }
}
