using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Mvc.Abstractions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class AcceptAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCreateAcceptance(BasarEntity basar, SellerEntity seller, int number, DateTime timestamp)
    {
        //  Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSession(basar)
            .With(_ => _.Seller, seller)
            .Create();
        ProductEntity[] products = Fixture.BuildProduct()
            .With(_ => _.Session, session)
            .With(_ => _.StorageState, StorageState.NotAccepted)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Acceptance)).Returns(number);
        Clock.Now = timestamp;
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, seller.Id)).DoesNothing();

        //  Act
        int id = await Sut.AcceptAsync(basar.Id, seller.Id, products.Ids());

        //  Assert
        TransactionEntity transaction = await Db.Transactions.FirstByIdAsync(id);
        using (new AssertionScope())
        {
            transaction.Basar.Should().BeEquivalentTo(basar);
            transaction.BasarId.Should().Be(basar.Id);
            transaction.CanCancel.Should().BeFalse();
            transaction.CanHasDocument.Should().BeTrue();
            transaction.Change.Should().BeNull();
            transaction.ChildTransactions.Should().BeEmpty();
            transaction.DocumentId.Should().BeNull();
            transaction.NeedsStatusPush.Should().BeTrue();
            transaction.Number.Should().Be(number);
            transaction.Notes.Should().BeNull();
            transaction.ParentTransaction.Should().BeNull();
            transaction.ParentTransactionId.Should().BeNull();
            transaction.Products.Should().HaveCount(products.Length);
            transaction.Seller.Should().BeEquivalentTo(seller);
            transaction.SellerId.Should().Be(seller.Id);
            transaction.TimeStamp.Should().Be(timestamp);
        }
            
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Acceptance)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, seller.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task SettledSeller_ShouldSetToNotSettled(BasarEntity basar, SellerEntity seller, int number, DateTime timestamp)
    {
        //  Arrange
        seller.ValueState = ValueState.Settled;
        AcceptSessionEntity session = Fixture.BuildAcceptSession(basar)
            .With(_ => _.Seller, seller)
            .Create();
        ProductEntity[] products = Fixture.BuildProduct()
            .With(_ => _.Session, session)
            .With(_ => _.StorageState, StorageState.NotAccepted)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Acceptance)).Returns(number);
        Clock.Now = timestamp;
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, seller.Id)).DoesNothing();

        //  Act
        int id = await Sut.AcceptAsync(basar.Id, seller.Id, products.Ids());

        //  Assert
        SellerEntity sellerFromDb = await Db.Sellers.FirstByIdAsync(seller.Id);
        sellerFromDb.ValueState.Should().Be(ValueState.NotSettled);

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Acceptance)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, seller.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NotAllowedProduct_ShouldThrow(BasarEntity basar, SellerEntity seller)
    {
        //  Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSession(basar)
            .With(_ => _.Seller, seller)
            .Create();
        ProductEntity[] products = Fixture.BuildProduct()
            .With(_ => _.Session, session)
            .With(_ => _.StorageState, StorageState.Available)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        //  Act
        Func<Task> act = async () => await Sut.AcceptAsync(basar.Id, seller.Id, products.Ids());

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
