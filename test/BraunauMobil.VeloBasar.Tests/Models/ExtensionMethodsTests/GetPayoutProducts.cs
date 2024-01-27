namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class GetPayoutProducts
{
    [Fact]
    public void ShouldReturnEmptyListIfNoProducts()
    {
        // Arrange
        List<ProductToTransactionEntity> productToTransactions = [];

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetPayoutProducts();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnSettledProductsThatAreSoldOrLost( ProductEntity soldProduct, ProductEntity lostProduct)
    {
        //  Arrange
        soldProduct.ValueState = ValueState.Settled;
        soldProduct.StorageState = StorageState.Sold;

        lostProduct.ValueState = ValueState.Settled;
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
        fixtureWithoutSettled.ExcludeEnumValues(ValueState.Settled);
        productToTransactions.AddRange(fixtureWithoutSettled.CreateMany<ProductToTransactionEntity>());

        VeloFixture fixtureWithoutSoldAndLost = new();
        fixtureWithoutSoldAndLost.ExcludeEnumValues(StorageState.Sold, StorageState.Lost);
        productToTransactions.AddRange(fixtureWithoutSoldAndLost.CreateMany<ProductToTransactionEntity>());

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetPayoutProducts();

        //  Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().Contain(soldProduct);
            result.Should().Contain(lostProduct);
        }
    }
}
