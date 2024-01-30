namespace BraunauMobil.VeloBasar.Tests.Models.SellerSearchModelTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        SellerSearchModel sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.FirstName.Should().BeNull();
            sut.LastName.Should().BeNull();
        }
    }
}
