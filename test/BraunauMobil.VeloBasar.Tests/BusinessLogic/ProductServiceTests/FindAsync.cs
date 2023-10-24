﻿namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class FindAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task EmptyDatabase_ReturnsNull(int productId)
    {
        //  Arrange

        //  Act
        ProductEntity? product = await Sut.FindAsync(productId);

        //  Assert
        product.Should().BeNull();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductExists_ReturnsProduct(ProductEntity product)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        ProductEntity? foundProduct = await Sut.FindAsync(product.Id);

        //  Assert
        foundProduct.Should().BeEquivalentTo(product);

        VerifyNoOtherCalls();
    }
}
