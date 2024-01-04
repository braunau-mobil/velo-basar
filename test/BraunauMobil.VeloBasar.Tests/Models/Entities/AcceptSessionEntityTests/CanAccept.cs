using BraunauMobil.VeloBasar.Tests;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AcceptSessionEntityTests;

public class CanAccept
{
    [Fact]
    public void NoProducts_ShouldBeFalse()
    {
        //  Arrange
        AcceptSessionEntity sut = new();

        //  Act
        bool result = sut.CanAccept();

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    public void WithProducts_ShouldBeTrue(int productCount)
    {
        //  Arrange
        VeloFixture fixture = new();
        AcceptSessionEntity sut = new();
        foreach (ProductEntity product in fixture.BuildProduct().CreateMany(productCount))
        {
            sut.Products.Add(product);
        }

        //  Act
        bool result = sut.CanAccept();

        //  Assert
        result.Should().BeTrue();
    }
}
