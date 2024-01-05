namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductToTransactionEntityTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void ShouldSetStateOfProduct(TransactionEntity transaction, ProductEntity product, int transactionId, int productId)
    {
        //  Arrange
        transaction.Id = transactionId;
        transaction.Type = TransactionType.Acceptance;
        product.Id = productId;
        product.ValueState = ValueState.NotSettled;
        product.StorageState = StorageState.NotAccepted;

        //  Act
        ProductToTransactionEntity sut = new(transaction, product);

        //  Assert
        using (new AssertionScope())
        {
            sut.Product.Should().Be(product);
            sut.ProductId.Should().Be(productId);
            sut.Transaction.Should().Be(transaction);
            sut.TransactionId.Should().Be(transactionId);

            product.StorageState.Should().Be(StorageState.Available);
            product.ValueState.Should().Be(ValueState.NotSettled);
        }
    }
}
