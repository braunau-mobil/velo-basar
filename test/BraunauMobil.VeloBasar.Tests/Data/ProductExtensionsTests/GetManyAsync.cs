using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductExtensionsTests;

public class GetManyAsync
    : DbTestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnOnlyProductsMatchinTheIds(ProductEntity[] productsToLookFor, ProductEntity[] otherProducts)
    {
        //  Arrange
        Db.Products.AddRange(productsToLookFor);
        Db.Products.AddRange(otherProducts);
        await Db.SaveChangesAsync();
        int[] productIds = productsToLookFor.Select(p => p.Id).ToArray();

        //  Act
        IReadOnlyList<ProductEntity> result = await Db.Products.GetManyAsync(productIds);

        //  Assert
        result.Should().BeEquivalentTo(productsToLookFor);
    }
}
