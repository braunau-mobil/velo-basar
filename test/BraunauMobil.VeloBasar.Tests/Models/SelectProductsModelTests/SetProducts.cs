namespace BraunauMobil.VeloBasar.Tests.Models.SelectProductsModelTests;

public class SetProducts
{
    private readonly SelectProductsModel _sut = new();

    [Fact]
    public void EmptyList()
    {
        //  Arrange
        IReadOnlyList<ProductEntity> products = Array.Empty<ProductEntity>();

        //  Act
        _sut.SetProducts(products);

        //  Assert
        _sut.Products.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public void ShouldCreateSelectModels(ProductEntity[] products)
    {
        //  Arrange

        //  Act
        _sut.SetProducts(products);

        //  Assert
        _sut.Products.Should().HaveCount(products.Length);
    }
}
