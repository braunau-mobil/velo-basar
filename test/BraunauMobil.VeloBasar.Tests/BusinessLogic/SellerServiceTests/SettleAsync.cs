using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class SettleAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task NoProducts_ShouldCreateTransactionAndSetSellerToSettled(int basarId, SellerEntity seller, int settlemendId)
    {
        //  Arrange
        IEnumerable<int> productIds = Enumerable.Empty<int>();
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();

        TransactionService.Setup(_ => _.SettleAsync(basarId, seller.Id, productIds))
            .ReturnsAsync(settlemendId);

        //  Act
        int result = await Sut.SettleAsync(basarId, seller.Id);

        //  Assert
        result.Should().Be(settlemendId);

        TransactionService.Verify(_ => _.SettleAsync(basarId, seller.Id, productIds), Times.Once);
        SellerEntity sellerInDb = await Db.Sellers.FirstByIdAsync(seller.Id);
        sellerInDb.ValueState.Should().Be(ValueState.Settled);
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task Products_ShouldCreateTransactionAndSetSellerToSettled(BasarEntity basar, SellerEntity seller, int settlemendId)
    {
        //  Arrange
        Fixture fixture = new();
        AcceptSessionEntity session = fixture.BuildAcceptSessionEntity()
            .With(_ => _.Basar, basar)
            .With(_ => _.Seller, seller)
            .Create();
        ProductEntity[] products = fixture.BuildProductEntity()
            .With(_ => _.Session, session)
            .Do(session.Products.Add)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        TransactionService.Setup(_ => _.SettleAsync(basar.Id, seller.Id, It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(settlemendId);

        //  Act
        int result = await Sut.SettleAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().Be(settlemendId);

        TransactionService.Verify(_ => _.SettleAsync(basar.Id, seller.Id, It.IsAny<IEnumerable<int>>()), Times.Once);
        SellerEntity sellerInDb = await Db.Sellers.FirstByIdAsync(seller.Id);
        sellerInDb.ValueState.Should().Be(ValueState.Settled);
        VerifyNoOtherCalls();
    }
}
