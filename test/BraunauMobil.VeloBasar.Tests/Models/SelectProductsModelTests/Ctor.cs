namespace BraunauMobil.VeloBasar.Tests.Models.SelectProductsModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        SelectProductsModel sut = new SelectProductsModel();

        //  Assert
        using (new AssertionScope())
        {
            sut.TransactionId.Should().Be(0);
            sut.Products.Should().BeEmpty();
        }
    }
}
