namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class GetPaybackProducts
{
    [Fact]
    public void ShouldReturnEmptyListIfNoProducts()
    {
        // Arrange
        List<ProductToTransactionEntity> productToTransactions = [];

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetPaybackProducts();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnNotSettledProductsThatAreSoldOrLost( ProductEntity soldProduct, ProductEntity lostProduct)
    {
        //  Arrange
        soldProduct.ValueState = ValueState.NotSettled;
        soldProduct.StorageState = StorageState.Sold;

        lostProduct.ValueState = ValueState.NotSettled;
        lostProduct.StorageState = StorageState.Lost;

        List<ProductToTransactionEntity> productToTransactions = 
        [
            new ProductToTransactionEntity()
            {
                Product = soldProduct
            },
            new ProductToTransactionEntity()
            {
                Product = lostProduct
            },
        ];

        VeloFixture fixtureWithoutSettled = new();
        fixtureWithoutSettled.ExcludeEnumValues(ValueState.NotSettled);
        productToTransactions.AddRange(fixtureWithoutSettled.CreateMany<ProductToTransactionEntity>());

        VeloFixture fixtureWithoutSoldAndLost = new();
        fixtureWithoutSoldAndLost.ExcludeEnumValues(StorageState.Sold, StorageState.Lost);
        productToTransactions.AddRange(fixtureWithoutSoldAndLost.CreateMany<ProductToTransactionEntity>());

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetPaybackProducts();

        //  Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().Contain(soldProduct);
            result.Should().Contain(lostProduct);
        }
    }
}
