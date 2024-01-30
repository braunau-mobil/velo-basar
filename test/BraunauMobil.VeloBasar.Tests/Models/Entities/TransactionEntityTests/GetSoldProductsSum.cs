namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class GetSoldProductsSum
{
    [Theory]
    [VeloAutoData]
    public void Empty_ShouldReturnZero(TransactionEntity sut)
    {
        //  Arrange

        //  Act
        decimal result = sut.GetSoldProductsSum();

        //  Assert
        result.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnPriceAllSoldProducts(TransactionEntity sut, ProductEntity soldProduct1, ProductEntity soldProduct2)
    {
        //  Arrange
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(StorageState.Sold);
        foreach (ProductToTransactionEntity productToTransactionEntity in fixture.BuildProductToTransactionEntity(sut).CreateMany())
        {
            sut.Products.Add(productToTransactionEntity);
        }
        soldProduct1.StorageState = StorageState.Sold;
        soldProduct1.ValueState = ValueState.Settled;
        sut.Products.Add(new ProductToTransactionEntity { Transaction = sut, Product = soldProduct1 });
        soldProduct2.StorageState = StorageState.Sold;
        soldProduct2.ValueState = ValueState.Settled;
        sut.Products.Add(new ProductToTransactionEntity { Transaction = sut, Product = soldProduct2 });

        //  Act
        decimal result = sut.GetSoldProductsSum();

        //  Assert  
        result.Should().Be(soldProduct1.Price + soldProduct2.Price);
    }
}
