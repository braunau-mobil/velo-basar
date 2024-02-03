using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductTypeExtensionsTests;

public class DefaultOrder
{
    [Fact]
    public void ShouldOrderByName()
    {
        // Arrange
        IQueryable<ProductTypeEntity> countries = new List<ProductTypeEntity>()
        {
            new () { Name = "DEU" },
            new () { Name = "AUT" },
            new () { Name = "CHE" }
        }.AsQueryable();

        // Act
        ProductTypeEntity[] result = countries.DefaultOrder().ToArray();

        // Assert
        result.Should().BeInAscendingOrder(c => c.Name);
    }
}
