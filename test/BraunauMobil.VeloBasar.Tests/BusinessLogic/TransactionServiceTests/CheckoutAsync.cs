﻿using FluentAssertions.Execution;
using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Mvc.Abstractions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class CheckoutAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task CreatesSaleTransactionWithoutDocument(BasarEntity basar, DateTime timestamp, int number)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSessionEntity()
            .With(_ => _.Basar, basar)
            .Create();
        ProductEntity[] products = Fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Available)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);
        await Db.SaveChangesAsync();

        Clock.Setup(_ => _.GetCurrentDateTime())
            .Returns(timestamp);
        NumberService.Setup(_ => _.NextNumberAsync(basar.Id, TransactionType.Sale))
            .ReturnsAsync(number);
        StatusPushService.Setup(_ => _.IsEnabled)
            .Returns(true);

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

        NumberService.Verify(_ => _.NextNumberAsync(basar.Id, TransactionType.Sale), Times.Once);
        StatusPushService.Verify(_ => _.IsEnabled, Times.Once);
        StatusPushService.Verify(_ => _.PushSellerAsync(basar.Id, session.SellerId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task LockedProduct_MustNotBeSold(BasarEntity basar)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSessionEntity()
            .With(_ => _.Basar, basar)
            .Create();
        ProductEntity product1 = Fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Locked)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .Create();
        Db.Products.Add(product1);
        ProductEntity product2 = Fixture.BuildProductEntity()
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

        VerifyNoOtherCalls();
    }
}
