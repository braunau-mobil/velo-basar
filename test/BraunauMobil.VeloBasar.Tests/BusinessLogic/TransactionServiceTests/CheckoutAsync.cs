using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Mvc.Abstractions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class CheckoutAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task CreatesSaleTransactionWithoutDocument(BasarEntity basar, DateTime timestamp, int number)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSession(basar).Create();
        ProductEntity[] products = Fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.Available)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Sale)).Returns(number);
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).DoesNothing();

        //  Act
        int result = await Sut.CheckoutAsync(basar.Id, products.Ids());

        //  Assert
        TransactionEntity saleFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            saleFromDb.Basar.Should().BeEquivalentTo(basar);
            saleFromDb.BasarId.Should().Be(basar.Id);
            saleFromDb.CanCancel.Should().BeTrue();
            saleFromDb.CanHasDocument.Should().BeTrue();
            saleFromDb.Change.Should().BeNull();
            saleFromDb.ChildTransactions.Should().BeEmpty();
            saleFromDb.DocumentId.Should().BeNull();
            saleFromDb.NeedsStatusPush.Should().BeTrue();
            saleFromDb.Number.Should().Be(number);
            saleFromDb.Notes.Should().BeNull();
            saleFromDb.ParentTransaction.Should().BeNull();
            saleFromDb.ParentTransactionId.Should().BeNull();
            saleFromDb.Products.Should().HaveCount(products.Length);
            saleFromDb.Seller.Should().BeNull();
            saleFromDb.SellerId.Should().BeNull();
            saleFromDb.TimeStamp.Should().Be(timestamp);
        }

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Sale)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task LockedProduct_MustNotBeSold(BasarEntity basar)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSession(basar)
            .Create();
        ProductEntity product1 = Fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.Locked)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .Create();
        Db.Products.Add(product1);
        ProductEntity product2 = Fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.Available)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .Create();
        Db.Products.Add(product2);
        await Db.SaveChangesAsync();

        //  Act
        Func<Task<int>> act = async () => await Sut.CheckoutAsync(basar.Id, new [] { product1.Id, product2.Id } );

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
