using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class SetLostAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task AvailbleAndSettled_ShouldSetLost(AcceptSessionEntity acceptSession, ProductEntity product, DateTime timestamp, int number, string notes)
    {
        // Arrange
        product.StorageState = StorageState.Available;
        product.ValueState = ValueState.NotSettled;
        acceptSession.Products.Add(product);
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(acceptSession.Basar.Id, TransactionType.SetLost)).Returns(number);
        
        //  Act
        int result = await Sut.SetLostAsync(acceptSession.Basar.Id, notes, product.Id);

        //  Assert
        TransactionEntity setLostFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            setLostFromDb.Basar.Should().BeEquivalentTo(acceptSession.Basar);
            setLostFromDb.BasarId.Should().Be(acceptSession.Basar.Id);
            setLostFromDb.CanCancel.Should().BeFalse();
            setLostFromDb.CanHasDocument.Should().BeFalse();
            setLostFromDb.Change.Should().BeNull();
            setLostFromDb.ChildTransactions.Should().BeEmpty();
            setLostFromDb.DocumentId.Should().BeNull();
            setLostFromDb.NeedsStatusPush.Should().BeFalse();
            setLostFromDb.Number.Should().Be(number);
            setLostFromDb.Notes.Should().Be(notes);
            setLostFromDb.ParentTransaction.Should().BeNull();
            setLostFromDb.ParentTransactionId.Should().BeNull();
            setLostFromDb.Products.Should().HaveCount(1);
            setLostFromDb.Seller.Should().BeNull();
            setLostFromDb.SellerId.Should().BeNull();
            setLostFromDb.TimeStamp.Should().Be(timestamp);
            setLostFromDb.Type.Should().Be(TransactionType.SetLost);
            ProductEntity productFromDb = setLostFromDb.Products.First().Product;
            productFromDb.Should().BeEquivalentTo(product);
            productFromDb.StorageState.Should().Be(StorageState.Lost);
            productFromDb.ValueState.Should().Be(ValueState.NotSettled);
        }

        A.CallTo(() => NumberService.NextNumberAsync(acceptSession.Basar.Id, TransactionType.SetLost)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NotAvailbleAndNotSettled_ShouldNotBeSetLosted(AcceptSessionEntity acceptSession, string notes)
    {
        // Arrange
        Fixture.ExcludeEnumValues(StorageState.Available);
        Fixture.ExcludeEnumValues(ValueState.NotSettled);
        ProductEntity product = Fixture.Create<ProductEntity>();
        acceptSession.Products.Add(product);
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        //  Act
        Func<Task<int>> act = async () => await Sut.SetLostAsync(acceptSession.Basar.Id, notes, product.Id);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
