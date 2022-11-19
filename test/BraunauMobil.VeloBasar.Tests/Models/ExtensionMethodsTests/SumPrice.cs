namespace BraunauMobil.VeloBasar.Tests.Models.ExtensionMethodsTests;

public class SumPrice
{
    [Fact]
    public void SumShouldBe()
    {
        ProductEntity[] products = new[]
        {
            new ProductEntity()
            {
                Price = 123.12m,
                StorageState = StorageState.Sold
            },
            new ProductEntity()
            {
                Price = 99.99m,
                StorageState = StorageState.Sold
            },
            new ProductEntity()
            {
                Price = 100.0m,
                StorageState = StorageState.Available
            }
        };
        products.SumPrice().Should().Be(323.11m);
    }
}
