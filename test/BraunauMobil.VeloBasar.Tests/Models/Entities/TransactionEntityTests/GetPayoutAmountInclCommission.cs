namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class GetPayoutAmountInclCommission
{
    [Theory]
    [VeloAutoData]
    public void Empty_ShouldReturnZero(TransactionEntity sut)
    {
        //  Arrange

        //  Act
        decimal result = sut.GetPayoutAmountInclCommission();

        //  Assert
        result.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnPriceOfAllPayoutProducts(TransactionEntity sut, ProductEntity soldProduct, ProductEntity lostProduct)
    {
        //  Arrange
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(StorageState.Sold, StorageState.Lost);
        fixture.ExcludeEnumValues(ValueState.Settled);
        foreach (ProductToTransactionEntity productToTransactionEntity in fixture.BuildProductToTransactionEntity(sut).CreateMany())
        {
            sut.Products.Add(productToTransactionEntity);
        }
        soldProduct.StorageState = StorageState.Sold;
        soldProduct.ValueState = ValueState.Settled;
        sut.Products.Add(new ProductToTransactionEntity { Transaction = sut, Product = soldProduct });
        lostProduct.StorageState = StorageState.Lost;
        lostProduct.ValueState = ValueState.Settled;
        sut.Products.Add(new ProductToTransactionEntity { Transaction = sut, Product = lostProduct });

        //  Act
        decimal result = sut.GetPayoutAmountInclCommission();

        //  Assert  
        result.Should().Be(soldProduct.Price + lostProduct.Price);
    }
}
