using BraunauMobil.VeloBasar.Routing;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class CancelRouterTests
{
    private readonly CancelRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToSelectSale()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSelectSale();

        //  Assert
        actual.Should().Be("//action=SelectSale&controller=Cancel");
    }

    [Fact]
    public void ToSelectProducts()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToSelectProducts(7777);

        //  Assert
        actual.Should().Be("//id=7777&action=SelectProducts&controller=Cancel");
    }
}
