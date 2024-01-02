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
    }

    [Fact]
    public void Products_ReturnsSumOfAcceptedProducts()
    {
        //  Arrange
        VeloFixture fixture = new();
        IEnumerable<ProductEntity> notAcceptedProducts = fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.NotAccepted)
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.NotAccepted);
        IEnumerable<ProductEntity> acceptedProducts = fixture.BuildProduct()
            .With(_ => _.Price, 1)
            .CreateMany();
        
        List<ProductEntity> products = new (notAcceptedProducts);
        products.AddRange(acceptedProducts);

        //  Act
        decimal result = Sut.GetAcceptedProductsAmount(products);

        //  Assert
        result.Should().Be(acceptedProducts.Count());
    }
}
