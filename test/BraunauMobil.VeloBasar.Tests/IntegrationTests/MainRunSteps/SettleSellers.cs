using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public class SettleSellers
    : TestStepBase
{
    public SettleSellers(TestContext testContext)
        : base(testContext)
    { }

    public override async Task Run()
    {
        await Do<SellerController>(async controller =>
        {
            IActionResult result = await controller.Settle(V.FirstBasar.Id, V.Sellers.Frodo.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//id=3&action=Success&controller=Transaction");
        });

        int transactionId = AssertDb(db =>
        {
            TransactionEntity transaction = db.AssertTransaction(V.FirstBasar.Id, TransactionType.Settlement, 1);
            transaction.BasarId.Should().Be(V.FirstBasar.Id);
            transaction.CanCancel.Should().BeFalse();
            transaction.CanHasDocument.Should().BeTrue();
            transaction.Change.Should().BeNull();
            transaction.ChildTransactions.Should().BeEmpty();
            transaction.DocumentId.Should().BeNull();
            transaction.NeedsBankingQrCodeOnDocument.Should().BeTrue();
            transaction.NeedsStatusPush.Should().BeTrue();
            transaction.Notes.Should().BeNull();
            transaction.Number.Should().Be(1);
            transaction.ParentTransaction.Should().BeNull();
            transaction.TimeStamp.Should().Be(Context.Clock.GetCurrentDateTime());
            transaction.SellerId.Should().Be(V.Sellers.Frodo.Id);
            transaction.Type.Should().Be(TransactionType.Settlement);
            transaction.UpdateDocumentOnDemand.Should().BeFalse();

            transaction.Products.Should().HaveCount(2);
            transaction.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Stahlross.Id);
            transaction.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Einrad.Id);

            V.Products.FirstBasar.AssertStorageStates(db, StorageState.Available, StorageState.Sold);
            V.Products.FirstBasar.AssertValueStates(db, ValueState.Settled, ValueState.Settled);

            db.Files.AsNoTracking().Should().HaveCount(2);

            return transaction.Id;
        });

        await Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Success(transactionId);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;

            model.AmountGiven.Should().Be(0);
            model.Entity.Should().NotBeNull();
            model.Entity.Id.Should().Be(transactionId);
            model.OpenDocument.Should().BeTrue();
            model.ShowAmountInput.Should().BeFalse();
            model.ShowChange.Should().BeTrue();
        });

        AssertDb(db =>
        {
            db.Files.AsNoTracking().Should().HaveCount(2);
        });

        //  Download document
        await Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Document(transactionId);

            FileContentResult file = result.Should().BeOfType<FileContentResult>().Subject;
            file.ContentType.Should().Be("application/pdf");
            file.FileDownloadName.Should().Be("2063-04-05T11:22:33_Settlement-3.pdf");
            file.FileContents.Should().NotBeNull();
        });

        AssertDb(db =>
        {
            TransactionEntity acceptance = db.Transactions.AsNoTracking().Should().Contain(f => f.Id == transactionId).Subject;

            acceptance.DocumentId.Should().NotBeNull();
        });
    }
}
