namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class NeedsBankingQrCodeOnDocument
{
    private readonly Fixture _fixture = new ();

    [Theory]
    [InlineAutoData(TransactionType.Acceptance)]
    [InlineAutoData(TransactionType.Cancellation)]
    [InlineAutoData(TransactionType.Lock)]
    [InlineAutoData(TransactionType.Sale)]
    [InlineAutoData(TransactionType.SetLost)]
    [InlineAutoData(TransactionType.Settlement)]
    [InlineAutoData(TransactionType.Unlock)]
    [InlineData(null, TransactionType.Acceptance)]
    [InlineData(null, TransactionType.Cancellation)]
    [InlineData(null, TransactionType.Lock)]
    [InlineData(null, TransactionType.Sale)]
    [InlineData(null, TransactionType.SetLost)]
    [InlineData(null, TransactionType.Settlement)]
    [InlineData(null, TransactionType.Unlock)]
    public void NoProducts_ShouldBeFalse(string? sellerIban, TransactionType transactionType)
    {
        //  Arrange
        TransactionEntity transaction = _fixture.BuildTransaction()
            .With(_ => _.Type, transactionType)
            .Create();
        transaction.Seller!.IBAN = sellerIban;
        transaction.Products.Clear();

        //  Act
        bool result = transaction.NeedsBankingQrCodeOnDocument;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData(TransactionType.Acceptance)]
    [InlineAutoData(TransactionType.Cancellation)]
    [InlineAutoData(TransactionType.Lock)]
    [InlineAutoData(TransactionType.Sale)]
    [InlineAutoData(TransactionType.SetLost)]
    [InlineAutoData(TransactionType.Settlement)]
    [InlineAutoData(TransactionType.Unlock)]
    [InlineData(null, TransactionType.Acceptance)]
    [InlineData(null, TransactionType.Cancellation)]
    [InlineData(null, TransactionType.Lock)]
    [InlineData(null, TransactionType.Sale)]
    [InlineData(null, TransactionType.SetLost)]
    [InlineData(null, TransactionType.Settlement)]
    [InlineData(null, TransactionType.Unlock)]
    public void NoSoldProducts_NoSellerIBAN_ShouldBeFalse(string? sellerIban, TransactionType transactionType)
    {
        //  Arrange
        TransactionEntity transaction = _fixture.BuildTransaction()
            .With(_ => _.Type, transactionType)
            .Create();
        transaction.Seller!.IBAN = sellerIban;
        AddUnsoldProducts(transaction);

        //  Act
        bool result = transaction.NeedsBankingQrCodeOnDocument;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData(TransactionType.Acceptance)]
    [InlineAutoData(TransactionType.Cancellation)]
    [InlineAutoData(TransactionType.Lock)]
    [InlineAutoData(TransactionType.Sale)]
    [InlineAutoData(TransactionType.SetLost)]
    [InlineAutoData(TransactionType.Settlement)]
    [InlineAutoData(TransactionType.Unlock)]
    public void SoldProducts_NoSellerIBAN_ShouldBeFalse(TransactionType transactionType)
    {
        //  Arrange
        TransactionEntity transaction = _fixture.BuildTransaction()
            .With(_ => _.Type, transactionType)
            .Create();
        transaction.Seller!.IBAN = null;

        AddSoldProducts(transaction);
        AddUnsoldProducts(transaction);

        _fixture.ExcludeEnumValues(StorageState.Sold);
        transaction.Products.AddMany(_fixture.BuildProductToTransactionEntity(transaction).With(_ => _.Product).Create, 10);

        //  Act
        bool result = transaction.NeedsBankingQrCodeOnDocument;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData(TransactionType.Acceptance)]
    [InlineAutoData(TransactionType.Cancellation)]
    [InlineAutoData(TransactionType.Lock)]
    [InlineAutoData(TransactionType.Sale)]
    [InlineAutoData(TransactionType.SetLost)]
    [InlineAutoData(TransactionType.Unlock)]
    public void SoldProducts_SellerIBAN_ShouldBeFalse(TransactionType transactionType)
    {
        //  Arrange
        TransactionEntity transaction = _fixture.BuildTransaction()
            .With(_ => _.Type, transactionType)
            .Create();
        transaction.Seller!.IBAN = _fixture.Create<string>();

        AddSoldProducts(transaction);
        AddUnsoldProducts(transaction);

        //  Act
        bool result = transaction.NeedsBankingQrCodeOnDocument;

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData(TransactionType.Settlement)]
    public void SoldProducts_SellerIBAN_ShouldBeTrue(TransactionType transactionType)
    {
        //  Arrange
        TransactionEntity transaction = _fixture.BuildTransaction()
            .With(_ => _.Type, transactionType)
            .Create();
        transaction.Seller!.IBAN = _fixture.Create<string>();

        AddSoldProducts(transaction);
        AddUnsoldProducts(transaction);

        //  Act
        bool result = transaction.NeedsBankingQrCodeOnDocument;

        //  Assert
        result.Should().BeTrue();
    }

    private void AddSoldProducts(TransactionEntity transaction)
    {
        IEnumerable<ProductEntity> soldProducts = _fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Sold)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .CreateMany();
        foreach (ProductEntity product in soldProducts)
        {
            transaction.Products.Add(new ProductToTransactionEntity(transaction, product));
        }
    }

    private void AddUnsoldProducts(TransactionEntity transaction)
    {
        _fixture.ExcludeEnumValues(StorageState.Sold);
        transaction.Products.AddMany(_fixture.BuildProductToTransactionEntity(transaction).With(_ => _.Product).Create, 10);
    }
}
