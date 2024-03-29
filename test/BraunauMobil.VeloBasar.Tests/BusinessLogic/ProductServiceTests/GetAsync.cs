﻿namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task EmptyDatabase_Throws(int productId)
    {
        //  Arrange

        //  Act
        Func<Task> act = async () => await Sut.GetAsync(productId);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductExists_ReturnsProduct(ProductEntity product)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        ProductEntity foundProduct = await Sut.GetAsync(product.Id);

        //  Assert
        foundProduct.Should().BeEquivalentTo(product);
    }
}
