using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Mvc.Abstractions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class SettleAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldSettleProducts(AcceptSessionEntity acceptSession, ProductEntity[] products, DateTime timestamp, int number)
    {
        // Arrange
        foreach (ProductEntity product in products)
        {
            product.StorageState = StorageState.Available;
            product.ValueState = ValueState.NotSettled;
            acceptSession.Products.Add(product);
        }
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(acceptSession.Basar.Id, TransactionType.Settlement)).Returns(number);
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(acceptSession.Basar.Id, acceptSession.Seller.Id)).DoesNothing();

        //  Act
        int result = await Sut.SettleAsync(acceptSession.Basar.Id, acceptSession.Seller.Id, products.Ids());

        //  Assert
        TransactionEntity settlementFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            settlementFromDb.Basar.Should().BeEquivalentTo(acceptSession.Basar);
            settlementFromDb.BasarId.Should().Be(acceptSession.Basar.Id);
            settlementFromDb.CanCancel.Should().BeFalse();
            settlementFromDb.CanHasDocument.Should().BeTrue();
            settlementFromDb.Change.Should().BeNull();
            settlementFromDb.ChildTransactions.Should().BeEmpty();
            settlementFromDb.DocumentId.Should().BeNull();
            settlementFromDb.NeedsStatusPush.Should().BeTrue();
            settlementFromDb.Number.Should().Be(number);
            settlementFromDb.Notes.Should().BeNull();
            settlementFromDb.ParentTransaction.Should().BeNull();
            settlementFromDb.ParentTransactionId.Should().BeNull();
            settlementFromDb.Seller.Should().Be(acceptSession.Seller);
            settlementFromDb.SellerId.Should().Be(acceptSession.Seller.Id);
            settlementFromDb.TimeStamp.Should().Be(timestamp);
            settlementFromDb.Type.Should().Be(TransactionType.Settlement);
            settlementFromDb.Products.Select(pt => pt.Product).Should().BeEquivalentTo(products);
            settlementFromDb.Products.Should().AllSatisfy(pt =>
            {
                pt.Product.StorageState.Should().Be(StorageState.Available);
                pt.Product.ValueState.Should().Be(ValueState.Settled);
            });
        }

        A.CallTo(() => NumberService.NextNumberAsync(acceptSession.Basar.Id, TransactionType.Settlement)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(acceptSession.Basar.Id, acceptSession.Seller.Id)).MustHaveHappenedOnceExactly();
    }
}
