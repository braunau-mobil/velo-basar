namespace BraunauMobil.VeloBasar.Tests.Models.ProductDetailsModelTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void CheckDefaults(ProductEntity product)
    {
        //  Arrange

        //  Act
        ProductDetailsModel sut = new(product);

        //  Assert
        using (new AssertionScope())
        {
            sut.CanLock.Should().BeFalse();
            sut.CanSetAsLost.Should().BeFalse();
            sut.CanUnlock.Should().BeFalse();
            sut.Entity.Should().Be(product);
            sut.Transactions.Should().BeEmpty();
        }
    }
}
