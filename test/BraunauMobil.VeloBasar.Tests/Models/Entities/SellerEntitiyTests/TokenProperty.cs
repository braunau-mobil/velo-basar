namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntitiyTests;

public class TokenProperty
{
    [Fact]
    public void Default_ShouldBeGuid()
    {
        //  Arrange
        SellerEntity sut = new ();

        //  Act
        string result = sut.Token;

        //  Assert
        using (new AssertionScope())
        {
            Guid.Parse(result).Should().NotBeEmpty();
        }
    }
}
