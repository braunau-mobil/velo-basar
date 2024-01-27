namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class GetProductsToPickup
{
    [Fact]
    public void ShouldReturnEmptyListIfNoProducts()
    {
        // Arrange
        List<ProductToTransactionEntity> productToTransactions = [];

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetProductsToPickup();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnSettledProductsThatAreSoldOrLost( ProductEntity notSoldProduct, ProductEntity lockedProduct)
    {
        //  Arrange
        notSoldProduct.ValueState = ValueState.Settled;
        notSoldProduct.StorageState = StorageState.Available;

        lockedProduct.ValueState = ValueState.Settled;
        lockedProduct.StorageState = StorageState.Locked;

        List<ProductToTransactionEntity> productToTransactions = 
        [
            new ProductToTransactionEntity()
            {
                Product = notSoldProduct
            },
            new ProductToTransactionEntity()
            {
                Product = lockedProduct
            },
        ];

        VeloFixture fixtureWithoutSettled = new();
        fixtureWithoutSettled.ExcludeEnumValues(ValueState.Settled);
        productToTransactions.AddRange(fixtureWithoutSettled.CreateMany<ProductToTransactionEntity>());

        VeloFixture fixtureWithoutAvailableAndLocked = new();
        fixtureWithoutAvailableAndLocked.ExcludeEnumValues(StorageState.Available, StorageState.Locked);
        productToTransactions.AddRange(fixtureWithoutAvailableAndLocked.CreateMany<ProductToTransactionEntity>());

        // Act
        IEnumerable<ProductEntity> result = productToTransactions.GetProductsToPickup();

        //  Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().Contain(notSoldProduct);
            result.Should().Contain(lockedProduct);
        }
    }
}
