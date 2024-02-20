namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class LockAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task TransactionServiceLockAsyncIsCalled(ProductEntity product, string notes, int id)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        A.CallTo(() => TransactionService.LockAsync(product.Session.BasarId, notes, product.Id)).Returns(id);

        //  Act
        await Sut.LockAsync(product.Id, notes);

        //  Assert
        A.CallTo(() => TransactionService.LockAsync(product.Session.BasarId, notes, product.Id)).MustHaveHappenedOnceExactly();
    }
}
