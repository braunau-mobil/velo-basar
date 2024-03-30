using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Mvc.Abstractions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class UnsettleAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldResetProductValueStatesAndSetSellerAsUnsettled(BasarEntity basar, SellerEntity seller, DateTime timestamp, int number)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSession(basar, seller).Create();
        AddProduct(session, StorageState.Available, ValueState.NotSettled);
        AddProduct(session, StorageState.Locked, ValueState.NotSettled);
        AddProduct(session, StorageState.Sold, ValueState.NotSettled);
        AddProduct(session, StorageState.Lost, ValueState.NotSettled);
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Unsettlement)).Returns(number);
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Settlement)).Returns(number);
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).DoesNothing();

        int settlementId = await Sut.SettleAsync(basar.Id, session.SellerId, Db.Products.ToArray().Ids());

        //  Act
        int result = await Sut.UnsettleAsync(settlementId);

        //  Assert
        TransactionEntity unsettlementFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            unsettlementFromDb.Basar.Should().BeEquivalentTo(basar);
            unsettlementFromDb.BasarId.Should().Be(basar.Id);
            unsettlementFromDb.CanCancel.Should().BeFalse();
            unsettlementFromDb.CanHasDocument.Should().BeFalse();
            unsettlementFromDb.Change.Should().BeNull();
            unsettlementFromDb.ChildTransactions.Should().BeEmpty();
            unsettlementFromDb.DocumentId.Should().BeNull();
            unsettlementFromDb.NeedsStatusPush.Should().BeTrue();
            unsettlementFromDb.Number.Should().Be(number);
            unsettlementFromDb.Notes.Should().BeNull();
            unsettlementFromDb.ParentTransactionId.Should().Be(settlementId);
            unsettlementFromDb.Products.Should().HaveCount(4);
            unsettlementFromDb.Seller.Should().BeEquivalentTo(seller);
            unsettlementFromDb.SellerId.Should().Be(seller.Id);
            unsettlementFromDb.TimeStamp.Should().Be(timestamp);
            unsettlementFromDb.Type.Should().Be(TransactionType.Unsettlement);

            ProductEntity[] products = await Db.Products.ToArrayAsync();
            products.Should().HaveCount(4);
            products.Should().AllSatisfy(product => product.ValueState.Should().Be(ValueState.NotSettled));
            products[0].StorageState.Should().Be(StorageState.Available);
            products[1].StorageState.Should().Be(StorageState.Locked);
            products[2].StorageState.Should().Be(StorageState.Sold);
            products[3].StorageState.Should().Be(StorageState.Lost);

            SellerEntity sellerFromDb = await Db.Sellers.FirstByIdAsync(seller.Id);
            sellerFromDb.ValueState.Should().Be(ValueState.NotSettled);
        }

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Unsettlement)).MustHaveHappenedOnceExactly();
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Settlement)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedTwiceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).MustHaveHappenedTwiceExactly();
    }

    private void AddProduct(AcceptSessionEntity acceptSession, StorageState storageState, ValueState valueState)
    {
        ProductEntity product = Fixture.BuildProduct()
            .With(_ => _.StorageState, storageState)
            .With(_ => _.ValueState, valueState)
            .With(_ => _.Session, acceptSession)
            .Create();
        acceptSession.Products.Add(product);
    }
}
