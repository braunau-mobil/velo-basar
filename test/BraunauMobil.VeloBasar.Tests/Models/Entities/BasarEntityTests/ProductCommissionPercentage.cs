namespace BraunauMobil.VeloBasar.Tests.Models.Entities.BasarEntityTests;

public class ProductCommissionPercentage
{
    [Theory]
    [InlineData(0.0, 0)]
    [InlineData(0.2, 20)]
    [InlineData(1.0, 100)]
    public void Get(decimal productCommision, int exptectedPercentage)
    {
        BasarEntity sut = new ()
        {
            ProductCommission = productCommision
        };
        sut.ProductCommissionPercentage.Should().Be(exptectedPercentage);
    }

    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(20, 0.2)]
    [InlineData(100, 1.0)]
    public void Set(int valueToSet, decimal exptectedCommision)
    {
        BasarEntity sut = new ()
        {
            ProductCommissionPercentage = valueToSet
        };
        sut.ProductCommission.Should().Be(exptectedCommision);
    }
}
