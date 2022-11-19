namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class GetCommissionAmount
{
    [Theory]
    [InlineData(100.0, 0)]
    public void NoCommission(decimal price, decimal commisionedPrice)
    {
        ProductEntity product = new()
        {
            Price = price
        };
        BasarEntity basar = new();
        Assert.Equal(commisionedPrice, product.GetCommissionAmount(basar));
    }

    [Theory]
    [InlineData(100.0, 10.0)]
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
        Assert.Equal(commisionedPrice, product.GetCommissionAmount(basar));
    }
}
