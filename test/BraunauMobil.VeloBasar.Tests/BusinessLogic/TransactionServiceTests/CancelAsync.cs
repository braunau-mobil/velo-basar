using Xan.AspNetCore.EntityFrameworkCore;
using Xan.AspNetCore.Mvc.Abstractions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class CancelAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task OneProductSaleDoesNotHaveDocumentYet_ShouldRemoveProductFromSaleAndCreateCancellationAndGenerateSaleDocument(BasarEntity basar, DateTime timestamp, int number, byte[] document)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.Create<AcceptSessionEntity>();
        ProductEntity[] products = Fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.NotAccepted)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);

        TransactionEntity sale = Fixture.BuildTransaction(basar)
            .Without(_ => _.Seller)
            .Without(_ => _.SellerId)
            .With(_ => _.Type, TransactionType.Sale)
            .Create();
        foreach (ProductEntity product in products)
        {
            ProductToTransactionEntity productToTransactionEntity = new(sale, product);
            sale.Products.Add(productToTransactionEntity);
        }
        Db.Transactions.Add(sale);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Cancellation)).Returns(number);
        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(A<TransactionEntity>.That.Matches(_ => _.Type == TransactionType.Sale))).Returns(document);
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).DoesNothing();

        //  Act
        int result = await Sut.CancelAsync(basar.Id, sale.Id, products.Take(1).Ids());

        //  Assert
        TransactionEntity cancellationFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            cancellationFromDb.Basar.Should().BeEquivalentTo(basar);
            cancellationFromDb.BasarId.Should().Be(basar.Id);
            cancellationFromDb.CanCancel.Should().BeFalse();
            cancellationFromDb.CanHasDocument.Should().BeFalse();
            cancellationFromDb.Change.Should().BeNull();
            cancellationFromDb.ChildTransactions.Should().BeEmpty();
            cancellationFromDb.DocumentId.Should().BeNull();
            cancellationFromDb.NeedsStatusPush.Should().BeTrue();
            cancellationFromDb.Number.Should().Be(number);
            cancellationFromDb.Notes.Should().BeNull();
            cancellationFromDb.ParentTransaction.Should().BeEquivalentTo(sale);
            cancellationFromDb.ParentTransactionId.Should().Be(sale.Id);
            cancellationFromDb.Products.Should().HaveCount(1);
            cancellationFromDb.Seller.Should().BeNull();
            cancellationFromDb.SellerId.Should().BeNull();
            cancellationFromDb.TimeStamp.Should().Be(timestamp);
        }
        TransactionEntity saleFromDb = await Db.Transactions.FirstByIdAsync(sale.Id);
        using (new AssertionScope())
        {
            saleFromDb.Basar.Should().BeEquivalentTo(sale.Basar);
            saleFromDb.BasarId.Should().Be(sale.Basar.Id);
            saleFromDb.CanCancel.Should().BeTrue();
            saleFromDb.CanHasDocument.Should().BeTrue();
            saleFromDb.ChildTransactions.Should().HaveCount(1);
            saleFromDb.ChildTransactions.Should().AllBeEquivalentTo(cancellationFromDb);
            saleFromDb.DocumentId.Should().Be(sale.DocumentId);
            saleFromDb.NeedsStatusPush.Should().BeTrue();
            saleFromDb.Number.Should().Be(sale.Number);
            saleFromDb.Notes.Should().Be(sale.Notes);
            saleFromDb.ParentTransaction.Should().BeNull();
            saleFromDb.ParentTransactionId.Should().BeNull();
            saleFromDb.Products.Should().HaveCount(products.Length - 1);
            saleFromDb.Seller.Should().BeEquivalentTo(sale.Seller);
            saleFromDb.SellerId.Should().Be(sale.SellerId);
            saleFromDb.TimeStamp.Should().Be(sale.TimeStamp);
        }

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Cancellation)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(A<TransactionEntity>.That.Matches(_ => _.Type == TransactionType.Sale))).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task OneProductSaleDoesHaveDocument_ShouldRemoveProductFromSaleAndCreateCancellationAndUpdateSaleDocument(BasarEntity basar, DateTime timestamp, int number, byte[] document, FileDataEntity saleDocument)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.Create<AcceptSessionEntity>();
        ProductEntity[] products = Fixture.BuildProduct()
            .With(_ => _.StorageState, StorageState.NotAccepted)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);

        TransactionEntity sale = Fixture.BuildTransaction(basar)
            .Without(_ => _.Seller)
            .Without(_ => _.SellerId)
            .With(_ => _.Type, TransactionType.Sale)
            .With(_ => _.DocumentId, saleDocument.Id)
            .Create();
        foreach (ProductEntity product in products)
        {
            ProductToTransactionEntity productToTransactionEntity = new(sale, product);
            sale.Products.Add(productToTransactionEntity);
        }
        Db.Transactions.Add(sale);
        Db.Files.Add(saleDocument);
        await Db.SaveChangesAsync();

        Clock.Now = timestamp;
        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Cancellation)).Returns(number);
        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(A<TransactionEntity>.That.Matches(_ => _.Type == TransactionType.Sale))).Returns(document);
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).DoesNothing();

        //  Act
        int result = await Sut.CancelAsync(basar.Id, sale.Id, products.Take(1).Ids());

        //  Assert
        TransactionEntity cancellationFromDb = await Db.Transactions.FirstByIdAsync(result);
        using (new AssertionScope())
        {
            cancellationFromDb.Basar.Should().BeEquivalentTo(basar);
            cancellationFromDb.BasarId.Should().Be(basar.Id);
            cancellationFromDb.CanCancel.Should().BeFalse();
            cancellationFromDb.CanHasDocument.Should().BeFalse();
            cancellationFromDb.Change.Should().BeNull();
            cancellationFromDb.ChildTransactions.Should().BeEmpty();
            cancellationFromDb.DocumentId.Should().BeNull();
            cancellationFromDb.NeedsStatusPush.Should().BeTrue();
            cancellationFromDb.Number.Should().Be(number);
            cancellationFromDb.Notes.Should().BeNull();
            cancellationFromDb.ParentTransaction.Should().BeEquivalentTo(sale);
            cancellationFromDb.ParentTransactionId.Should().Be(sale.Id);
            cancellationFromDb.Products.Should().HaveCount(1);
            cancellationFromDb.Seller.Should().BeNull();
            cancellationFromDb.SellerId.Should().BeNull();
            cancellationFromDb.TimeStamp.Should().Be(timestamp);
        }
        TransactionEntity saleFromDb = await Db.Transactions.FirstByIdAsync(sale.Id);
        using (new AssertionScope())
        {
            saleFromDb.Basar.Should().BeEquivalentTo(sale.Basar);
            saleFromDb.BasarId.Should().Be(sale.Basar.Id);
            saleFromDb.CanCancel.Should().BeTrue();
            saleFromDb.CanHasDocument.Should().BeTrue();
            saleFromDb.ChildTransactions.Should().HaveCount(1);
            saleFromDb.ChildTransactions.Should().AllBeEquivalentTo(cancellationFromDb);
            saleFromDb.DocumentId.Should().Be(sale.DocumentId);
            saleFromDb.NeedsStatusPush.Should().BeTrue();
            saleFromDb.Number.Should().Be(sale.Number);
            saleFromDb.Notes.Should().Be(sale.Notes);
            saleFromDb.ParentTransaction.Should().BeNull();
            saleFromDb.ParentTransactionId.Should().BeNull();
            saleFromDb.Products.Should().HaveCount(products.Length - 1);
            saleFromDb.Seller.Should().BeEquivalentTo(sale.Seller);
            saleFromDb.SellerId.Should().Be(sale.SellerId);
            saleFromDb.TimeStamp.Should().Be(sale.TimeStamp);
        }

        A.CallTo(() => NumberService.NextNumberAsync(basar.Id, TransactionType.Cancellation)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatusPushService.PushSellerAsync(basar.Id, session.SellerId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(A<TransactionEntity>.That.Matches(_ => _.Type == TransactionType.Sale))).MustHaveHappenedOnceExactly();
    }
}
