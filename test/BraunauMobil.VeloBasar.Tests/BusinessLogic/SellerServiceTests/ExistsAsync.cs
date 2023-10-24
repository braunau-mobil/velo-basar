﻿namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class ExistsAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task DoesNotExist_ReturnsFalse(int sellerId)
    {
        //  Arrange

        //  Act
        bool result = await Sut.ExistsAsync(sellerId);

        //  Assert
        result.Should().BeFalse();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task Exists_ReturnsTrue(SellerEntity seller)
    {
        //  Arrange
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();

        //  Act
        bool result = await Sut.ExistsAsync(seller.Id);

        //  Assert
        result.Should().BeTrue();

        VerifyNoOtherCalls();
    }
}
