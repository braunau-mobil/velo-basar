namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class CanUnsettle
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance)]
    [VeloInlineAutoData(TransactionType.Cancellation)]
    [VeloInlineAutoData(TransactionType.Lock)]
    [VeloInlineAutoData(TransactionType.SetLost)]
    [VeloInlineAutoData(TransactionType.Settlement)]
    [VeloInlineAutoData(TransactionType.Unlock)]
    public void ShouldBeFalse(TransactionType type, TransactionEntity sut)
    {
        //  Arrange
        sut.Type = type;

        //  Act
        bool result = sut.CanUnsettle;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public void Settlement_NoProductsAllowCancellation_ShouldBeFalse(TransactionEntity sut)
    {
        //  Arrange
        sut.Type = TransactionType.Settlement;
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(ValueState.Settled);
        foreach (ProductToTransactionEntity productToTransactionEntity in fixture.BuildProductToTransactionEntity(sut).CreateMany())
        {
            sut.Products.Add(productToTransactionEntity);
        }

        //  Act
        bool result = sut.CanUnsettle;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public void Settlement_SomeProductsAllowCancellation_ShouldBeFalse(TransactionEntity sut, ProductEntity[] productsToCancel)
    {
        //  Arrange
        sut.Type = TransactionType.Settlement;
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(ValueState.Settled);
        foreach (ProductToTransactionEntity productToTransactionEntity in fixture.BuildProductToTransactionEntity(sut).CreateMany())
        {
            sut.Products.Add(productToTransactionEntity);
        }
        foreach (ProductEntity product in productsToCancel)
        {
            product.StorageState = StorageState.Sold;
            product.ValueState = ValueState.Settled;

            sut.Products.Add(new ProductToTransactionEntity(sut, product));
        }

        //  Act
        bool result = sut.CanUnsettle;

        //  Assert
        result.Should().BeTrue();
    }
}
