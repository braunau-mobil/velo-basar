namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class LockAsync
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task TransactionServiceLockAsyncIsCalled(ProductEntity product, string notes)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        await Sut.LockAsync(product.Id, notes);

        //  Assert
        TransactionService.Verify(_ => _.LockAsync(product.Session.BasarId, notes, product.Id));
    }
}
