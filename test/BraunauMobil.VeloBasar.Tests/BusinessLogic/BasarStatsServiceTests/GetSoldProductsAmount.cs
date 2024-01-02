namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductsAmount
    : TestBase<EmptySqliteDbFixture>
{
    [Fact]
    public void EmptyList_ReturnsZero()
    {
        //  Arrange
        IEnumerable<ProductEntity> products = Enumerable.Empty<ProductEntity>();

        //  Act
        decimal result = Sut.GetSoldProductsAmount(products);

        //  Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Products_ReturnsSumOfAcceptedProducts()
    {
        //  Arrange
        VeloFixture fixture = new();
        IEnumerable<ProductEntity> soldProducts = fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.Sold)
            .With(_ => _.Price, 1)
            .CreateMany();        

        fixture.ExcludeEnumValues(StorageState.Sold);
        IEnumerable<ProductEntity> notSoldProducts = fixture.BuildProduct()
            .CreateMany();
        
        List<ProductEntity> products = new (soldProducts);
        products.AddRange(notSoldProducts);

        //  Act
        decimal result = Sut.GetSoldProductsAmount(products);

        //  Assert
        result.Should().Be(soldProducts.Count());
    }
}
