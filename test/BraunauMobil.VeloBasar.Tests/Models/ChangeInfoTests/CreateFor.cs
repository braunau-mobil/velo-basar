namespace BraunauMobil.VeloBasar.Tests.Models.ChangeInfoTests;

public class CreateFor
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance)]
    public void ShouldReturnInvalid(TransactionType transactionType, decimal amount)
    {
        //  Arrange
        TransactionEntity transaction = new() { Type = transactionType };

        //  Act
        ChangeInfo changeInfo = ChangeInfo.CreateFor(transaction, amount);

        //  Assert
        using (new AssertionScope())
        {
            changeInfo.Amount.Should().Be(0);
            changeInfo.Denomination.Should().BeEmpty();
            changeInfo.HasDenomination.Should().BeFalse();
            changeInfo.IsValid.Should().BeFalse();
        }
    }

    [Theory]
    [VeloAutoData]
    public void Cancellation_ShouldReturnProductsSum(TransactionEntity transaction, ProductEntity product)
    {
        //  Arrange
        transaction.Type = TransactionType.Cancellation;
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product));


        //  Act
        ChangeInfo changeInfo = ChangeInfo.CreateFor(transaction, product.Price * 2);

        //  Assert
        using (new AssertionScope())
        {
            changeInfo.Amount.Should().Be(transaction.GetProductsSum());
            changeInfo.Denomination.Should().NotBeEmpty();
            changeInfo.HasDenomination.Should().BeTrue();
            changeInfo.IsValid.Should().BeTrue();
        }
    }

    [Theory]
    [VeloAutoData]
    public void Settlement_ShouldReturnSoldTotal(TransactionEntity transaction, ProductEntity product1, ProductEntity product2)
    {
        //  Arrange
        transaction.Basar.ProductCommission = 0.1m;
        transaction.Type = TransactionType.Settlement;
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product1));
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product2));
        product1.StorageState = StorageState.Sold;
        product2.StorageState = StorageState.Available;

        //  Act
        ChangeInfo changeInfo = ChangeInfo.CreateFor(transaction, product1.Price * 10);

        //  Assert
        using (new AssertionScope())
        {
            changeInfo.Amount.Should().Be(transaction.GetSoldTotal());
            changeInfo.Denomination.Should().NotBeEmpty();
            changeInfo.HasDenomination.Should().BeTrue();
            changeInfo.IsValid.Should().BeTrue();
        }
    }

    [Theory]
    [VeloAutoData]
    public void Sale_LessGiven_ShouldReturnInvalid(TransactionEntity transaction, ProductEntity product1, ProductEntity product2)
    {
        //  Arrange
        transaction.Basar.ProductCommission = 0.1m;
        transaction.Type = TransactionType.Sale;
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product1));
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product2));
        product1.StorageState = StorageState.Sold;
        product2.StorageState = StorageState.Sold;
        decimal amountGiven = product1.Price + product2.Price - 0.1m;

        //  Act
        ChangeInfo changeInfo = ChangeInfo.CreateFor(transaction, amountGiven);

        //  Assert
        using (new AssertionScope())
        {
            changeInfo.Amount.Should().Be(0);
            changeInfo.Denomination.Should().BeEmpty();
            changeInfo.HasDenomination.Should().BeFalse();
            changeInfo.IsValid.Should().BeFalse();
        }
    }

    [Theory]
    [VeloAutoData]
    public void Sale_EqualGiven_ShouldReturnZero(TransactionEntity transaction, ProductEntity product1, ProductEntity product2)
    {
        //  Arrange
        transaction.Basar.ProductCommission = 0.1m;
        transaction.Type = TransactionType.Sale;
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product1));
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product2));
        product1.StorageState = StorageState.Sold;
        product2.StorageState = StorageState.Sold;
        decimal amountGiven = product1.Price + product2.Price;

        //  Act
        ChangeInfo changeInfo = ChangeInfo.CreateFor(transaction, amountGiven);

        //  Assert
        using (new AssertionScope())
        {
            changeInfo.Amount.Should().Be(0);
            changeInfo.Denomination.Should().NotBeEmpty();
            changeInfo.HasDenomination.Should().BeFalse();
            changeInfo.IsValid.Should().BeTrue();
        }
    }

    [Theory]
    [VeloAutoData]
    public void Sale_MoreGiven_ShouldReturnAdditionalAmountGiven(TransactionEntity transaction, ProductEntity product1, ProductEntity product2, decimal additionalAmountGiven)
    {
        //  Arrange
        transaction.Basar.ProductCommission = 0.1m;
        transaction.Type = TransactionType.Sale;
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product1));
        transaction.Products.Add(new ProductToTransactionEntity(transaction, product2));
        product1.StorageState = StorageState.Sold;
        product2.StorageState = StorageState.Sold;
        decimal amountGiven = product1.Price + product2.Price + additionalAmountGiven;

        //  Act
        ChangeInfo changeInfo = ChangeInfo.CreateFor(transaction, amountGiven);

        //  Assert
        using (new AssertionScope())
        {
            changeInfo.Amount.Should().Be(additionalAmountGiven);
            changeInfo.Denomination.Should().NotBeEmpty();
            changeInfo.HasDenomination.Should().BeTrue();
            changeInfo.IsValid.Should().BeTrue();
        }
    }
}
