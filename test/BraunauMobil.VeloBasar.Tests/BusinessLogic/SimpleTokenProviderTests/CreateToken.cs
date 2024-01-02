using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SimpleTokenProviderTests;

public class CreateToken
{
    [Theory]
    [VeloAutoData]
    public void ShouldReturnFirstNameIdLastNameAsHex(SellerEntity seller)
    {
        //  Arrange
        SimpleTokenProvider sut = new();
        seller.FirstName = "Abcd";
        seller.LastName = "xYz";
        seller.Id = 666;

        //  Act
        string result = sut.CreateToken(seller);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("416229A7859");
        }
    }
}
