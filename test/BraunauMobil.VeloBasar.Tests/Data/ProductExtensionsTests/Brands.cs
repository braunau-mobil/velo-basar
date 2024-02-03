using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductExtensionsTests;

public class Brands
{
    [Fact]
    public void Brands_Should_ReturnDistinctBrands()
    {
        // Arrange
        IQueryable<ProductEntity> products = new List<ProductEntity>
        {
            new() { Brand = "Brand1" },
            new() { Brand = "Brand2" },
            new() { Brand = "Brand2" },
            new() { Brand = "Brand3" },
            new() { Brand = "Brand3" },
            new() { Brand = "Brand3" }
        }.AsQueryable();

        // Act
        IEnumerable<string> result = products.Brands();

        // Assert
        result.Should().BeEquivalentTo(new[] { "Brand1", "Brand2", "Brand3" });
    }
}
