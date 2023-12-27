namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetManyAsync_Paginated
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task EmptyDatabase_ReturnsEmptyList(int pageSize, int pageIndex, int basarId, string searchString, StorageState storageState, ValueState valueState, string brand, int productTypeId)
    {
        //  Arrange

        //  Act
        IReadOnlyCollection<ProductEntity> products = await Sut.GetManyAsync(pageSize, pageIndex, basarId, searchString, storageState, valueState, brand, productTypeId);

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
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSameBrand(BasarEntity basar, ProductEntity[] products, string brand)
    {
        //  Arrange
        await InsertProductsAsync(basar, products, product =>
        {
            product.Brand = brand;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, null, null, brand, null);

        //  Assert
        result.Should().BeEquivalentTo(products);
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
    }

    [Theory]
    [AutoData]
    public async Task ProductsExist_ReturnsAllWithSame(BasarEntity basar, ProductEntity[] products, StorageState storageState, ValueState valueState, string brand, ProductTypeEntity productType)
    {
        //  Arrange
        Db.ProductTypes.Add(productType);
        await InsertProductsAsync(basar, products, product =>
        {
            product.StorageState = storageState;
            product.ValueState = valueState;
            product.Brand = brand;
            product.Type = productType;
        });

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, string.Empty, storageState, valueState, brand, productType.Id);

        //  Assert
        result.Should().BeEquivalentTo(products);
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
