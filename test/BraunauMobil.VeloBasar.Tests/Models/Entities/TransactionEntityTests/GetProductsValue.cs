namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class GetProductsValue
{
    [Theory]
    [VeloAutoData]
    public void Empty_ShouldReturnZero(TransactionEntity sut)
    {
        //  Arrange

        //  Act
        decimal result = sut.GetProductsValue();

        //  Assert
        result.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public void ShouldReturnPriceOfAllProducts(TransactionEntity sut, ProductEntity[] products)
    {
        //  Arrange
        foreach (ProductEntity product in products)
        {
            sut.Products.Add(new ProductToTransactionEntity(sut, product));
        }
        
        //  Act
        decimal result = sut.GetProductsValue();

        //  Assert  
        result.Should().Be(products.SumPrice());
    }
}
