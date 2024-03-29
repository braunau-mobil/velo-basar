﻿namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class GetAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithId_ProductIsReturnedAndRelationsAreLoaded(ProductEntity product)
    {
        // Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        // Act
        AcceptProductModel result = await Sut.GetAsync(product.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.CanAccept.Should().BeTrue();
            result.Entity.Should().BeEquivalentTo(product);
            result.Entity.Brand.Should().NotBeNull();
            result.Entity.Type.Should().NotBeNull();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task WithSessionIdAndProduct_SessionIsLoadedAndProductIsReturned(ProductEntity product)
    {
        // Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        // Act
        AcceptProductModel result = await Sut.GetAsync(product.Session.Id, product);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.CanAccept.Should().BeTrue();
            result.Entity.Should().Be(product);
            result.Entity.Brand.Should().NotBeNull();
            result.Entity.Type.Should().NotBeNull();
        }
    }
}
