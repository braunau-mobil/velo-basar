namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetManyAsync_ListOfIds
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task EmptyDatabase_ReturnsEmptyList(int[] productIds)
    {
        //  Arrange

        //  Act
        IReadOnlyList<ProductEntity> products = await Sut.GetManyAsync(productIds);

        //  Assert
        products.Should().BeEmpty();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductExists_EmptyIdList_ReturnsEmptyList(ProductEntity[] products)
    {
        //  Arrange
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyCollection<ProductEntity> result = await Sut.GetManyAsync(new List<int>());

        //  Assert
        result.Should().BeEmpty();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductExists_ReturnsListOfProducts(ProductEntity p1, ProductEntity p2, ProductEntity p3, int productIdThatDoesNotExist)
    {
        //  Arrange
        Db.Products.AddRange(p1, p2, p3);
        await Db.SaveChangesAsync();
        int[] ids = new []
        {
            p1.Id,
            p2.Id,
            p3.Id,
            productIdThatDoesNotExist
        };

        //  Act
        IReadOnlyCollection<ProductEntity> products = await Sut.GetManyAsync(ids);

        //  Assert
        products.Should().HaveCount(3);
        products.Should().ContainEquivalentOf(p1);
        products.Should().ContainEquivalentOf(p2);
        products.Should().ContainEquivalentOf(p3);

        VerifyNoOtherCalls();
    }
}
