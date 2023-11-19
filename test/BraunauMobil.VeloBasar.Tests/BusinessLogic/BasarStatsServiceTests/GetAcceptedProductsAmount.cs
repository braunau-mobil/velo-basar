namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetAcceptedProductsAmount
    : TestBase<EmptySqliteDbFixture>
{
    [Fact]
    public void EmptyList_ReturnsZero()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        decimal result = Sut.GetAcceptedProductsAmount(products);

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
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.NotAccepted);
        IEnumerable<ProductEntity> acceptedProducts = fixture.BuildProductEntity()
            .With(_ => _.Price, 1)
            .CreateMany();
        
        List<ProductEntity> products = new (notAcceptedProducts);
        products.AddRange(acceptedProducts);

        //  Act
        decimal result = Sut.GetAcceptedProductsAmount(products);

        //  Assert
        result.Should().Be(acceptedProducts.Count());

        VerifyNoOtherCalls();
    }
}
