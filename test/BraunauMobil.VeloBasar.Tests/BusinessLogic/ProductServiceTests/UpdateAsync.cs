using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class UpdateAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ProductGetsUpdated(ProductEntity product, string newDescription)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        product.Description = newDescription;
        await Sut.UpdateAsync(product);

        //  Assert
        ProductEntity updatedProduct = await Db.Products.FirstByIdAsync(product.Id);
        updatedProduct.Should().BeEquivalentTo(product);
    }
}
