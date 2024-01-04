using BraunauMobil.VeloBasar.Tests;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AcceptSessionEntityTests;

public class Products
{
    [Fact]
    public void ShouldBeInitialized()
    {
        //  Act & Arrange
        AcceptSessionEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Products.Should().NotBeNull();
            sut.Products.Should().BeEmpty();
        }
    }
}
