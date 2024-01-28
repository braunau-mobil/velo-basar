namespace BraunauMobil.VeloBasar.Tests.Models.AcceptProductModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        AcceptProductModel sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.CanAccept.Should().BeFalse();
            sut.Entity.Should().BeNull();
            sut.Products.Should().BeNull();
            sut.SellerId.Should().Be(0);
            sut.SessionId.Should().Be(0);
        }
    }
}
