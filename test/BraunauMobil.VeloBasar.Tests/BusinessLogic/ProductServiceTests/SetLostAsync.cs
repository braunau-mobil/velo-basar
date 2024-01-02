namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class SetLostAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task TransactionServiceSetLostAsyncIsCalled(ProductEntity product, string notes, int id)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        A.CallTo(() => TransactionService.SetLostAsync(product.Session.BasarId, notes, product.Id)).Returns(id);

        //  Act
        await Sut.SetLostAsync(product.Id, notes);

        //  Assert
        A.CallTo(() => TransactionService.SetLostAsync(product.Session.BasarId, notes, product.Id)).MustHaveHappenedOnceExactly();
    }
}
