namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class GetCommissionedPrice
{
    [Theory]
    [InlineData(100.0, 100.0)]
    public void NoCommission(decimal price, decimal commisionedPrice)
    {
        ProductEntity product = new()
        {
            Price = price
        };
        BasarEntity basar = new();
        Assert.Equal(commisionedPrice, product.GetCommissionedPrice(basar));
    }
    [Theory]
    [InlineData(100.0, 90.0)]
    public void TenPercent(decimal price, decimal commisionedPrice)
    {
        ProductEntity product = new()
        {
            Price = price
        };
        BasarEntity basar = new()
        {
            ProductCommission = 0.1m
        };
        Assert.Equal(commisionedPrice, product.GetCommissionedPrice(basar));
    }
}
