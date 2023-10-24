namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class SetLostAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task TransactionServiceSetLostAsyncIsCalled(ProductEntity product, string notes)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        await Sut.SetLostAsync(product.Id, notes);

        //  Assert
        TransactionService.Verify(_ => _.SetLostAsync(product.Session.BasarId, notes, product.Id));
        VerifyNoOtherCalls();
    }
}
