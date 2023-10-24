namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class UnlockAsync
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task TransactionServiceUnlockAsyncIsCalled(ProductEntity product, string notes)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        await Sut.UnlockAsync(product.Id, notes);

        //  Assert
        TransactionService.Verify(_ => _.UnlockAsync(product.Session.BasarId, notes, product.Id));
    }
}
