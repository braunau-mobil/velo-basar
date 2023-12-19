using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Tests.Mockups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public class SellProducts
    : TestStepBase
{
    public SellProducts(TestContext testContext)
        : base(testContext)
    { }

    public override async Task Run()
    {
        CartModel cart = await Do<CartController, CartModel>(async controller =>
        {
            IActionResult result = await controller.Index();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            return view.ViewData.Model.Should().BeOfType<CartModel>().Subject;
        });

        cart.ActiveBasarId.Should().Be(0);
        cart.HasProducts.Should().BeFalse();
        cart.ProductId.Should().Be(0);
        cart.Products.Should().BeEmpty();

        cart.ProductId = V.Products.FirstBasar.Frodo.Stahlross.Id;

        await Do<CartController>(async controller =>
        {
            IActionResult result = await controller.Add(cart);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("Index");
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            view.ViewData.Model.Should().Be(cart);
        });

        cart.ActiveBasarId.Should().Be(0);
        cart.HasProducts.Should().BeTrue();
        cart.ProductId.Should().Be(0);
        cart.Products.Should().HaveCount(1);
        cart.Products.Should().Contain(p => p.Id == V.Products.FirstBasar.Frodo.Stahlross.Id);

        await Do<CartController>(async controller =>
        {
            IActionResult result = await controller.Checkout(V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//id=2&action=Success&controller=Transaction");
        });

        CartCookie cartCookie = new(Context.Services.GetRequiredService<CookiesMock>(), Context.Services.GetRequiredService<CookiesMock>());
        cartCookie.GetCart().Should().BeEmpty();

        int saleId = AssertDb(db =>
        {
            TransactionEntity transaction = db.AssertTransaction(V.FirstBasar.Id, TransactionType.Sale, 1);
            transaction.BasarId.Should().Be(V.FirstBasar.Id);
            transaction.CanCancel.Should().BeTrue();
            transaction.CanHasDocument.Should().BeTrue();
            transaction.Change.Should().BeNull();
            transaction.ChildTransactions.Should().BeEmpty();
            transaction.DocumentId.Should().BeNull();
            transaction.HasDocument.Should().BeFalse();
            transaction.NeedsBankingQrCodeOnDocument.Should().BeFalse();
            transaction.NeedsStatusPush.Should().BeTrue();
            transaction.Notes.Should().BeNull();
            transaction.Number.Should().Be(1);
            transaction.ParentTransaction.Should().BeNull();
            transaction.TimeStamp.Should().Be(Context.Clock.GetCurrentDateTime());
            transaction.SellerId.Should().BeNull();
            transaction.Type.Should().Be(TransactionType.Sale);

            transaction.Products.Should().HaveCount(1);
            transaction.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Stahlross.Id);

            V.Products.FirstBasar.AssertStorageStates(db, StorageState.Available, StorageState.Sold);
            V.Products.FirstBasar.AssertValueStates(db, ValueState.NotSettled, ValueState.NotSettled);

            db.Files.AsNoTracking().Should().HaveCount(1);

            return transaction.Id;
        });        

        await Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Success(saleId);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;

            model.AmountGiven.Should().Be(0);
            model.Entity.Should().NotBeNull();
            model.Entity.Id.Should().Be(saleId);
            model.OpenDocument.Should().BeTrue();
            model.ShowAmountInput.Should().BeTrue();
            model.ShowChange.Should().BeFalse();
        });

        AssertDb(db =>
        {
            db.Files.AsNoTracking().Should().HaveCount(1);
        });

        //  Download document
        await Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Document(saleId);

            FileContentResult file = result.Should().BeOfType<FileContentResult>().Subject;
            file.ContentType.Should().Be("application/pdf");
            file.FileDownloadName.Should().Be("2063-04-05T11:22:33_Sale-2.pdf");
            file.FileContents.Should().NotBeNull();
        });

        AssertDb(db =>
        {
            TransactionEntity acceptance = db.Transactions.AsNoTracking().Should().Contain(f => f.Id == saleId).Subject;

            acceptance.DocumentId.Should().NotBeNull();
        });
    }
}
