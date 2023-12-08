using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class UpdateAsync
	: TestBase<EmptySqliteDbFixture>
{
	[Theory]
	[AutoData]
	public async Task ProductIsAttachedAndUpdated(ProductEntity productToInsert, string brand, ProductTypeEntity productType, ProductEntity productToUpdate)
	{
		//	Arrange
		Db.Products.Add(productToInsert);
		Db.ProductTypes.Add(productType);
		await Db.SaveChangesAsync();
		Db.ChangeTracker.Clear();

		productToUpdate.Id = productToInsert.Id;
		productToUpdate.Brand = brand;
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
        updated.Brand = initial.Brand;
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
