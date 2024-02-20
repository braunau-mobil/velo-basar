using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class SettleAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task NoProducts_ShouldCreateTransactionAndSetSellerToSettled(int basarId, SellerEntity seller, int settlemendId)
    {
        //  Arrange
        IEnumerable<int> productIds = Enumerable.Empty<int>();
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();

        A.CallTo(() => TransactionService.SettleAsync(basarId, seller.Id, productIds)).Returns(settlemendId);

        //  Act
        int result = await Sut.SettleAsync(basarId, seller.Id);

        //  Assert
        result.Should().Be(settlemendId);

        A.CallTo(() => TransactionService.SettleAsync(basarId, seller.Id, productIds)).MustHaveHappenedOnceExactly();
        SellerEntity sellerInDb = await Db.Sellers.FirstByIdAsync(seller.Id);
        sellerInDb.ValueState.Should().Be(ValueState.Settled);
    }

    [Theory]
    [VeloAutoData]
    public async Task Products_ShouldCreateTransactionAndSetSellerToSettled(BasarEntity basar, SellerEntity seller, int settlemendId)
    {
        //  Arrange
        VeloFixture fixture = new();
        AcceptSessionEntity session = fixture.BuildAcceptSession(basar)
            .With(_ => _.Seller, seller)
            .Create();
        ProductEntity[] products = fixture.BuildProduct()
            .With(_ => _.Session, session)
            .Do(session.Products.Add)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        A.CallTo(() => TransactionService.SettleAsync(basar.Id, seller.Id, A<IEnumerable<int>>._)).Returns(settlemendId);

        //  Act
        int result = await Sut.SettleAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be(settlemendId);

        A.CallTo(() => TransactionService.SettleAsync(basar.Id, seller.Id, A<IEnumerable<int>>._)).MustHaveHappenedOnceExactly();
        SellerEntity sellerInDb = await Db.Sellers.FirstByIdAsync(seller.Id);
        sellerInDb.ValueState.Should().Be(ValueState.Settled);
    }
}
