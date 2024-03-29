﻿using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class CreateAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ProductWithoutSession_ThrowsInvalidOperationException(ProductEntity product)
    {
        //  Arrange
        product.SessionId = 0;

        //  Act
        Func<Task> act = async () => await Sut.CreateAsync(product);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidProduct_CreatesProductAndReloadsItsRelations(string brand, ProductTypeEntity productType, AcceptSessionEntity acceptSession, ProductEntity product)
    {
        //  Arrange
        Db.ProductTypes.Add(productType);
        Db.AcceptSessions.Add(acceptSession);

        await Db.SaveChangesAsync();

        product.Brand = null!;
        product.Brand = brand;
        product.Session = null!;
        product.SessionId = acceptSession.Id;
        product.Type = null!;
        product.TypeId = productType.Id;

        //  Act
        await Sut.CreateAsync(product);

        //  Assert
        using (new AssertionScope())
        {
            ProductEntity result = await Db.Products.FirstByIdAsync(product.Id);
            result.Should().NotBeNull();
            result.Brand.Should().BeEquivalentTo(brand);
            result.Type.Should().BeEquivalentTo(productType);
        }
    }
}
