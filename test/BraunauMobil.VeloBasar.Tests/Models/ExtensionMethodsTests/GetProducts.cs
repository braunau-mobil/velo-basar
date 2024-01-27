namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class GetProducts
{
    [Fact]
    public void ShouldReturnEmptyListIfNoProducts()
    {
        // Arrange
        List<ProductToTransactionEntity> productToTransactions = [];

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetProducts();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnSettledProductsThatAreSoldOrLost(ProductEntity[] products)
    {
        //  Arrange
        List<ProductToTransactionEntity> productToTransactions = products.Select(product => new ProductToTransactionEntity()
        {
            Product = product
        }).ToList();

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetProducts();

        //  Assert
        result.Should().BeEquivalentTo(products);
    }
}
