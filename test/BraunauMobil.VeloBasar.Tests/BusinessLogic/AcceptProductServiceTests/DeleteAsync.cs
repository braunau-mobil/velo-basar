namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class DeleteAsync
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ProductIsDeleted(ProductEntity product)
    {
        // Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        // Act
        await Sut.DeleteAsync(product.Id);

        // Assert
        Db.Products.Should().BeEmpty();
    }
}
