namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductTypeServiceTests;

public class CanDeleteAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ProductUsesCountry_ShouldReturnFalse(ProductEntity product)
    {
        // Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        // Act
        bool result = await Sut.CanDeleteAsync(product.Type);

        // Assert
        result.Should().BeFalse();
    }


    [Theory]
    [VeloAutoData]
    public async Task NoSellerUsesCountry_ShouldReturnTrue(ProductEntity procuct, ProductTypeEntity toCheck)
    {
        // Arrange
        Db.Products.Add(procuct);
        Db.ProductTypes.Add(toCheck);
        await Db.SaveChangesAsync();

        // Act
        bool result = await Sut.CanDeleteAsync(toCheck);

        // Assert
        result.Should().BeTrue();
    }
}
