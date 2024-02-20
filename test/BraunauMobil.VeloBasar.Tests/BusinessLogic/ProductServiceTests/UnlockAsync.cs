namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class UnlockAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task TransactionServiceUnlockAsyncIsCalled(ProductEntity product, string notes, int id)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        A.CallTo(() => TransactionService.UnlockAsync(product.Session.BasarId, notes, product.Id)).Returns(id);

        //  Act
        await Sut.UnlockAsync(product.Id, notes);

        //  Assert
        A.CallTo(() => TransactionService.UnlockAsync(product.Session.BasarId, notes, product.Id)).MustHaveHappenedOnceExactly();
    }
}
