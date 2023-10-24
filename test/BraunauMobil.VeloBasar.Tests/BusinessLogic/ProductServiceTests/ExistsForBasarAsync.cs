namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class ExistsForBasarAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task EmptyDatabase_ReturnsFalse(int basarId, int productId)
    {
        //  Arrange

        //  Act
        bool result = await Sut.ExistsForBasarAsync(basarId, productId);

        //  Assert
        result.Should().BeFalse();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task BasarExistsButNoProducts_ReturnsFalse(BasarEntity basar, int productId)
    {
        //  Arrange
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();

        //  Act
        bool result = await Sut.ExistsForBasarAsync(basar.Id, productId);

        //  Assert
        result.Should().BeFalse();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task BasarAndProductExist_ReturnsTrue(ProductEntity product)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        bool result = await Sut.ExistsForBasarAsync(product.Session.BasarId, product.Id);

        //  Assert
        result.Should().BeTrue();

        VerifyNoOtherCalls();
    }
}
