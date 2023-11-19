namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetManyAsync_Paginated
    : TestBase<EmptySqliteDbFixture>
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

        VerifyNoOtherCalls();
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

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSameStorageState(BasarEntity basar, ProductEntity[] products, StorageState storageState)
    {
        //  Arrange
        await InsertProductsAsync(basar, products, product =>
        {
            product.StorageState = storageState;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, storageState, null, null, null);

        //  Assert
        result.Should().BeEquivalentTo(products);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSameValueState(BasarEntity basar, ProductEntity[] products, ValueState valueState)
    {
        //  Arrange
        await InsertProductsAsync(basar, products, product => 
        {
            product.ValueState = valueState;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, null, valueState, null, null);

        //  Assert
        result.Should().BeEquivalentTo(products);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSameBrandId(BasarEntity basar, ProductEntity[] products, BrandEntity brand)
    {
        //  Arrange
        Db.Brands.Add(brand);
        await InsertProductsAsync(basar, products, product =>
        {
            product.Brand = brand;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, null, null, brand.Id, null);

        //  Assert
        result.Should().BeEquivalentTo(products);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSameProductTypeId(BasarEntity basar, ProductEntity[] products, ProductTypeEntity productType)
    {
        //  Arrange
        Db.ProductTypes.Add(productType);
        await InsertProductsAsync(basar, products, product =>
        {
            product.Type = productType;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, null, null, null, productType.Id);

        //  Assert
        result.Should().BeEquivalentTo(products);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSame(BasarEntity basar, ProductEntity[] products, StorageState storageState, ValueState valueState, BrandEntity brand, ProductTypeEntity productType)
    {
        //  Arrange
        Db.Brands.Add(brand);
        Db.ProductTypes.Add(productType);
        await InsertProductsAsync(basar, products, product =>
        {
            product.StorageState = storageState;
            product.ValueState = valueState;
            product.Brand = brand;
            product.Type = productType;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, storageState, valueState, brand.Id, productType.Id);

        //  Assert
        result.Should().BeEquivalentTo(products);

        VerifyNoOtherCalls();
    }

    private async Task InsertProductsAsync(BasarEntity basar, ProductEntity[] products, Action<ProductEntity> adjustProduct)
    {
        //  Arrange
        Db.Basars.Add(basar);
        foreach (ProductEntity product in products)
        {
            adjustProduct(product);
            product.Session.Basar = basar;
            product.Session.BasarId = 0;
        }
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();
    }
}
