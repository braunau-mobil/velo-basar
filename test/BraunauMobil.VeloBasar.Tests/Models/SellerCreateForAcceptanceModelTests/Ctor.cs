namespace BraunauMobil.VeloBasar.Tests.Models.SellerCreateForAcceptanceModelTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void WithSeller_ShouldNotHaveSearchedAndSellersShouldBeEmpty(SellerEntity seller)
    {
        //  Arrange

        //  Act
        SellerCreateForAcceptanceModel sut = new(seller);

        //  Asert
        using (new AssertionScope())
        {
            sut.FoundSellers.Should().BeEmpty();
            sut.HasSearched.Should().BeFalse();
            sut.Seller.Should().Be(seller);
        }
    }

    [Theory]
    [VeloAutoData]
    public void WithSellerAndSearched_ShouldNotHaveSearchedAndSellersShouldBeEmpty(SellerEntity seller, bool hasSearched, SellerEntity[] foundSellers)
    {
        //  Arrange

        //  Act
        SellerCreateForAcceptanceModel sut = new(seller, hasSearched, foundSellers);

        //  Asert
        using (new AssertionScope())
        {
            sut.FoundSellers.Should().BeEquivalentTo(foundSellers);
            sut.HasSearched.Should().Be(hasSearched);
            sut.Seller.Should().Be(seller);
        }
    }
}
