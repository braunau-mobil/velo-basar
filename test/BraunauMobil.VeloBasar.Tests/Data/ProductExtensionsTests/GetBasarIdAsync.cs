using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductExtensionsTests;

public class GetBasarIdAsync
    : DbTestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task GetBasarIdAsync_WhenCalled_ReturnsBasarId(ProductEntity productToLookFor, ProductEntity[] otherProducts)
    {
        // Arrange
        Db.Products.Add(productToLookFor);
        Db.Products.AddRange(otherProducts);
        await Db.SaveChangesAsync();

        // Act
        int result = await Db.Products.GetBasarIdAsync(productToLookFor.Id);

        // Assert
        result.Should().Be(productToLookFor.Session.BasarId);
    }
}
