namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class GetSoldProducts
{
    [Fact]
    public void ShouldReturnEmptyListIfNoProducts()
    {
        // Arrange
        List<ProductToTransactionEntity> productToTransactions = [];

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetSoldProducts();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnSettledProductsThatAreSoldOrLost(ProductEntity soldProduct1, ProductEntity soldProduct2)
    {
        //  Arrange
        VeloFixture fixture = new();
        ProductEntity[] soldProducts = fixture.BuildProduct()
            .With(product => product.StorageState, StorageState.Sold)
            .CreateMany().ToArray();
        
        List<ProductToTransactionEntity> productToTransactions = [];
        productToTransactions.AddRange(soldProducts.Select(product => new ProductToTransactionEntity()
        {
            Product = product
        }));
        
        VeloFixture fixtureWithoutSold = new();
        fixtureWithoutSold.ExcludeEnumValues(StorageState.Sold);
        productToTransactions.AddRange(fixtureWithoutSold.CreateMany<ProductToTransactionEntity>());

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetSoldProducts();

        //  Assert
        result.Should().BeEquivalentTo(soldProducts);
    }
}
