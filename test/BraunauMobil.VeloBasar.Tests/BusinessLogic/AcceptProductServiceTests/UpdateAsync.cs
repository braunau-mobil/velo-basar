using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class UpdateAsync
	: TestBase<EmptySqliteDbFixture>
{
	[Theory]
	[AutoData]
	public async Task ProductIsAttachedAndUpdated(ProductEntity productToInsert, BrandEntity brand, ProductTypeEntity productType, ProductEntity productToUpdate)
	{
		//	Arrange
		Db.Products.Add(productToInsert);
		Db.Brands.Add(brand);
		Db.ProductTypes.Add(productType);
		await Db.SaveChangesAsync();
		Db.ChangeTracker.Clear();

		productToUpdate.Id = productToInsert.Id;
		productToUpdate.BrandId = brand.Id;
		productToUpdate.Brand = null!;
		productToUpdate.TypeId = productType.Id;
		productToUpdate.Type = null!;
		productToUpdate.SessionId = productToInsert.SessionId;
		productToUpdate.Session = null!;

		//	Act
		await Sut.UpdateAsync(productToUpdate);

		//	Assert
		ProductEntity updatedProduct = await Db.Products.FirstByIdAsync(productToInsert.Id);
		updatedProduct.Should().BeEquivalentTo(productToUpdate);
	}

    [Theory]
    [AutoData]
    public async Task RelationsAreReloaded(ProductEntity initial, ProductEntity updated)
    {
        //  Arrange
        Db.Products.Add(initial);
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();

        updated.Id = initial.Id;
        updated.BrandId = initial.BrandId;
        updated.Brand = null!;
        updated.TypeId = initial.TypeId;
        updated.Type = null!;
        updated.SessionId = initial.SessionId;
        updated.Session = null!;

        //  Arrange
        await Sut.UpdateAsync(updated);

        //  Assert
        updated.Id.Should().Be(initial.Id);
        updated.Brand.Should().BeEquivalentTo(initial.Brand);
        updated.Type.Should().BeEquivalentTo(initial.Type);
        updated.Session.Should().NotBeNull();
        updated.Should().NotBeEquivalentTo(initial);
    }
}
