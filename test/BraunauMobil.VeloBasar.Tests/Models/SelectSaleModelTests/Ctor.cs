namespace BraunauMobil.VeloBasar.Tests.Models.SelectSaleModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        SelectSaleModel sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.SaleNumber.Should().Be(0);
            sut.Sale.Should().BeNull();
        }
    }
}
