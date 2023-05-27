using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;


public class UpdateAsync
	: TestBase
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
}

