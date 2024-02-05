using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class LockAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task AvailbleAndSettled_ShouldLock(AcceptSessionEntity acceptSession, ProductEntity product, DateTime timestamp, int number, string notes)
    {
        // Arrange
        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;
        acceptSession.Products.Add(product);
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(acceptSession.Basar.Id, TransactionType.Lock)).Returns(number);
        
        //  Act
        int result = await Sut.LockAsync(acceptSession.Basar.Id, notes, product.Id);

        //  Assert
        TransactionEntity lockFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            lockFromDb.Basar.Should().BeEquivalentTo(acceptSession.Basar);
            lockFromDb.BasarId.Should().Be(acceptSession.Basar.Id);
            lockFromDb.CanCancel.Should().BeFalse();
            lockFromDb.CanHasDocument.Should().BeFalse();
            lockFromDb.Change.Should().BeNull();
            lockFromDb.ChildTransactions.Should().BeEmpty();
            lockFromDb.DocumentId.Should().BeNull();
            lockFromDb.NeedsStatusPush.Should().BeFalse();
            lockFromDb.Number.Should().Be(number);
            lockFromDb.Notes.Should().Be(notes);
            lockFromDb.ParentTransaction.Should().BeNull();
            lockFromDb.ParentTransactionId.Should().BeNull();
            lockFromDb.Products.Should().HaveCount(1);
            lockFromDb.Seller.Should().BeNull();
            lockFromDb.SellerId.Should().BeNull();
            lockFromDb.TimeStamp.Should().Be(timestamp);
            lockFromDb.Type.Should().Be(TransactionType.Lock);
            ProductEntity productFromDb = lockFromDb.Products.First().Product;
            productFromDb.Should().BeEquivalentTo(product);
            productFromDb.StorageState.Should().Be(StorageState.Locked);
            productFromDb.ValueState.Should().Be(ValueState.NotSettled);
        }

        A.CallTo(() => NumberService.NextNumberAsync(acceptSession.Basar.Id, TransactionType.Lock)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NotAvailbleAndNotSettled_ShouldNotBeLocked(AcceptSessionEntity acceptSession, string notes)
    {
        // Arrange
        Fixture.ExcludeEnumValues(StorageState.Available);
        Fixture.ExcludeEnumValues(ValueState.NotSettled);
        ProductEntity product = Fixture.Create<ProductEntity>();
        acceptSession.Products.Add(product);
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        //  Act
        Func<Task<int>> act = async () => await Sut.LockAsync(acceptSession.Basar.Id, notes, product.Id);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
