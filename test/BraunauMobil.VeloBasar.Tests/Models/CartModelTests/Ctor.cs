namespace BraunauMobil.VeloBasar.Tests.Models.CartModelTests;

public class Defaults
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        CartModel model = new();

        //  Assert
        using (new AssertionScope())
        {
            model.HasProducts.Should().BeFalse();
            model.ProductId.Should().Be(0);
            model.Products.Should().BeEmpty();
        }
    }
}
