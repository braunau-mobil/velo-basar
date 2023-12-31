using BraunauMobil.VeloBasar.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class CartRouterTests
{
    private readonly CartRouter _sut = new(new LinkGeneratorMock());

    [Fact]
    public void ToAdd()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToAdd();

        //  Assert
        actual.Should().Be("//action=Add&controller=Cart");
    }

    [Fact]
    public void ToCheckout()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToCheckout();

        //  Assert
        actual.Should().Be("//action=Checkout&controller=Cart");
    }

    [Fact]
    public void ToClear()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToClear();

        //  Assert
        actual.Should().Be("//action=Clear&controller=Cart");
    }

    [Fact]
    public void ToDelete()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToDelete(333);

        //  Assert
        actual.Should().Be("//productId=333&action=Delete&controller=Cart");
    }

    [Fact]
    public void ToIndex()
    {
        //  Arrange

        //  Act
        string actual = _sut.ToIndex();

        //  Assert
        actual.Should().Be("//action=Index&controller=Cart");
    }

}
