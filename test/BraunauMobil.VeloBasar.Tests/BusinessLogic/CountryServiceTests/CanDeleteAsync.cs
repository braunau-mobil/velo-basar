namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.CountryServiceTests;

public class CanDeleteAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task SellerUsesCountry_ShouldReturnFalse(SellerEntity seller)
    {
        // Arrange
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();

        // Act
        bool result = await Sut.CanDeleteAsync(seller.Country);

        // Assert
        result.Should().BeFalse();
    }


    [Theory]
    [VeloAutoData]
    public async Task NoSellerUsesCountry_ShouldReturnTrue(SellerEntity seller, CountryEntity toCheck)
    {
        // Arrange
        Db.Sellers.Add(seller);
        Db.Countries.Add(toCheck);
        await Db.SaveChangesAsync();

        // Act
        bool result = await Sut.CanDeleteAsync(toCheck);

        // Assert
        result.Should().BeTrue();
    }
}
