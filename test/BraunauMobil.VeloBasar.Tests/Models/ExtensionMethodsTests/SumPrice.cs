namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class SumPrice
{
    [Fact]
    public void ShouldReturnEmptyListIfNoProducts()
    {
        // Arrange
        List<ProductEntity> productToTransactions = [];

        // Act
        decimal result = productToTransactions.SumPrice();

        // Assert
        result.Should().Be(decimal.Zero);
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnSettledProductsThatAreSoldOrLost(ProductEntity[] products)
    {
        //  Arrange
        decimal expectedSum = products.Sum(product => product.Price);

        // Act
        decimal result = products.SumPrice();

        //  Assert
        result.Should().Be(expectedSum);
    }
}
