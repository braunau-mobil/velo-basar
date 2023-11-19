namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetAcceptedProductsCount
    : TestBase<EmptySqliteDbFixture>
{
    [Fact]
    public void EmptyList_ReturnsZero()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        int result = Sut.GetAcceptedProductsCount(products);

        //  Assert
        result.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Fact]
    public void Products_ReturnsSumOfAcceptedProducts()
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<ProductEntity> notAcceptedProducts = fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.NotAccepted)
            .CreateMany()
            .ToList();

        fixture.ExcludeEnumValues(StorageState.NotAccepted);
        IEnumerable<ProductEntity> acceptedProducts = fixture.BuildProductEntity()
            .CreateMany();
        
        List<ProductEntity> products = new (notAcceptedProducts);
        products.AddRange(acceptedProducts);

        //  Act
        int result = Sut.GetAcceptedProductsCount(products);

        //  Assert
        result.Should().Be(acceptedProducts.Count());

        VerifyNoOtherCalls();
    }
}
