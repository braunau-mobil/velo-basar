using BraunauMobil.VeloBasar.Data;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductExtensionsTests;

public class IncludeAll
    : DbTestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task AllRelationsShouldBeLoaded(ProductEntity product)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();

        //  Act
        ProductEntity result = await Db.Products.IncludeAll().FirstByIdAsync(product.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBe(product);
            result.Should().NotBeNull();
            result.Type.Should().NotBeNull();
            result.Session.Should().NotBeNull();
            result.Session.Basar.Should().NotBeNull();
            result.Session.Seller.Should().NotBeNull();
            result.Session.Seller.Country.Should().NotBeNull();
        }
    }
}
