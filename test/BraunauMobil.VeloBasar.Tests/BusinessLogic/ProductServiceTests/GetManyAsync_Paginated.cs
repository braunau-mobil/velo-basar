using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetManyAsync_Paginated
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task EmptyDatabase_ReturnsEmptyList(int pageSize, int pageIndex, int basarId, string searchString, StorageState storageState, ValueState valueState, int brandId, int productTypeId)
    {
        //  Arrange

        //  Act
        IReadOnlyCollection<ProductEntity> products = await Sut.GetManyAsync(pageSize, pageIndex, basarId, searchString, storageState, valueState, brandId, productTypeId);

        //  Assert
        products.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAll(BasarEntity basar, ProductEntity[] products)
    {
        //  Arrange
        Db.Basars.Add(basar);
        foreach (ProductEntity product in products)
        {
            product.Session.Basar = basar;
            product.Session.BasarId = 0;
        }
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, null, null, null, null);

        //  Assert
        result.Should().BeEquivalentTo(products);
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSameValueState(BasarEntity basar, ProductEntity[] products, StorageState storageState)
    {
        //  Arrange
        Db.Basars.Add(basar);
        foreach (ProductEntity product in products)
        {
            product.StorageState = storageState;
            product.Session.Basar = basar;
            product.Session.BasarId = 0;
        }
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, storageState, null, null, null);

        //  Assert
        result.Should().BeEquivalentTo(products);
    }
}
