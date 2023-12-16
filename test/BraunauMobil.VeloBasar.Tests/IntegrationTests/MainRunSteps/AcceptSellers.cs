using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public static class AcceptSellers
{
    public static async Task Run(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        //  Create seller
        await context.Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.Start(V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=CreateForAcceptance&controller=Seller");
        });

        SellerCreateForAcceptanceModel createModel = await context.Do<SellerController, SellerCreateForAcceptanceModel>(async controller =>
        {
            int? sellerId = null;
            IActionResult result = await controller.CreateForAcceptance(sellerId);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            return view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
        });

        createModel.Seller.FirstName = "Frodo";
        createModel.Seller.LastName = "Beutlin";
        createModel.Seller.CountryId = V.Countries.Austria.Id;
        createModel.Seller.ZIP = "5280";
        createModel.Seller.City = "Braunau";
        createModel.Seller.Street = "Am Stadtplatz 1";
        createModel.Seller.PhoneNumber = "0664 1234567";
        createModel.Seller.EMail = "frodo.beutlin@shirenet.at";
        createModel.Seller.HasNewsletterPermission = true;
        createModel.Seller.IBAN = "AT036616345714985792";
        createModel.Seller.BankAccountHolder = "Ing. Frodo Beutlin";

        await context.Do<SellerController>(async controller =>
        {
            IActionResult result = await controller.CreateForAcceptance(createModel.Seller);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sellerId=1&action=StartForSeller&controller=AcceptSession");
        });

        context.AssertDb(db =>
        {
            SellerEntity frodo = db.Sellers.AsNoTracking().Should().Contain(s => s.FirstName == "Frodo" && s.LastName == "Beutlin").Subject;
            V.Sellers.Frodo = frodo;
        });

        //  Start accept session
        await context.Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.StartForSeller(V.Sellers.Frodo.Id, V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sessionId=1&action=Create&controller=AcceptProduct");
        });

        int acceptSessionId = 0;
        context.AssertDb(db =>
        {
            AcceptSessionEntity session = db.AcceptSessions.AsNoTracking().Should().Contain(s => s.SellerId == V.Sellers.Frodo.Id && s.BasarId == V.FirstBasar.Id).Subject;
            acceptSessionId = session.Id;
        });

        await EnterProducts(context, acceptSessionId);

        //  Finish accept session
        await context.Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.Submit(acceptSessionId);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//id=1&action=Success&controller=Transaction");
        });

        int acceptanceId = context.AssertDb(db =>
        {
            TransactionEntity acceptance = db.Transactions
                .Include(t => t.Products)
                .AsNoTracking().Should().Contain(t => t.SellerId == V.Sellers.Frodo.Id && t.Type == TransactionType.Acceptance).Subject;
            acceptance.BasarId.Should().Be(V.FirstBasar.Id);
            acceptance.CanCancel.Should().BeFalse();
            acceptance.CanHasDocument.Should().BeTrue();
            acceptance.Change.Should().BeNull();
            acceptance.ChildTransactions.Should().BeEmpty();
            acceptance.DocumentId.Should().BeNull();
            acceptance.HasDocument.Should().BeFalse();
            acceptance.NeedsBankingQrCodeOnDocument.Should().BeFalse();
            acceptance.NeedsStatusPush.Should().BeTrue();
            acceptance.Notes.Should().BeNull();
            acceptance.Number.Should().Be(1);
            acceptance.ParentTransaction.Should().BeNull();
            acceptance.TimeStamp.Should().Be(context.Clock.GetCurrentDateTime());
            acceptance.SellerId.Should().Be(V.Sellers.Frodo.Id);    
            acceptance.Type.Should().Be(TransactionType.Acceptance);
            
            acceptance.Products.Should().HaveCount(2);
            acceptance.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Stahlross.Id);
            acceptance.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Einrad.Id);

            db.AssertProductStates(V.Products.FirstBasar.Frodo.Stahlross.Id, StorageState.Available, ValueState.NotSettled);
            db.AssertProductStates(V.Products.FirstBasar.Frodo.Einrad.Id, StorageState.Available, ValueState.NotSettled);

            db.Files.AsNoTracking().Should().BeEmpty();

            return acceptance.Id;
        });

        await context.Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Success(acceptanceId);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            TransactionSuccessModel model = view.Model.Should().BeOfType<TransactionSuccessModel>().Subject;

            model.AmountGiven.Should().Be(0);
            model.Entity.Should().NotBeNull();
            model.Entity.Id.Should().Be(acceptanceId);
            model.OpenDocument.Should().BeTrue();
            model.ShowAmountInput.Should().BeFalse();
            model.ShowChange.Should().BeFalse();
        });

        context.AssertDb(db =>
        {
            db.Files.AsNoTracking().Should().BeEmpty();
        });

        //  Download document
        await context.Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Document(acceptanceId);

            FileContentResult file = result.Should().BeOfType<FileContentResult>().Subject;
            file.ContentType.Should().Be("application/pdf");
            file.FileDownloadName.Should().Be("2063-04-05T11:22:33_Acceptance-1.pdf");
            file.FileContents.Should().NotBeNull();
        });

        context.AssertDb(db =>
        {
            TransactionEntity acceptance = db.Transactions.AsNoTracking().Should().Contain(f => f.Id == acceptanceId).Subject;

            acceptance.DocumentId.Should().NotBeNull();
        });
    }
    private static async Task EnterProducts(TestContext context, int acceptSessionId)
    {
        V.Products.FirstBasar.Frodo.Stahlross = await context.EnterProduct(acceptSessionId, model =>
        {
            model.CanAccept.Should().BeFalse();
            model.SellerId.Should().Be(V.Sellers.Frodo.Id);
            model.SessionId.Should().Be(acceptSessionId);
            model.Products.Should().BeEmpty();

            model.Entity.TypeId = V.ProductTypes.Stahlross.Id;
            model.Entity.Brand = "Simplon";
            model.Entity.Color = "Schwarz";
            model.Entity.FrameNumber = "1234567890";
            model.Entity.Description = "Gepäckträger, Korb";
            model.Entity.TireSize = "26";
            model.Entity.Price = 120;
        });

        V.Products.FirstBasar.Frodo.Einrad = await context.EnterProduct(acceptSessionId, model =>
        {
            model.CanAccept.Should().BeTrue();
            model.SellerId.Should().Be(V.Sellers.Frodo.Id);
            model.SessionId.Should().Be(acceptSessionId);
            model.Products.Should().HaveCount(1);

            model.Entity.TypeId = V.ProductTypes.Einrad.Id;
            model.Entity.Brand = "AJATA";
            model.Entity.Color = "Rot";
            model.Entity.FrameNumber = "qe340t9ni-0i4";
            model.Entity.Description = "";
            model.Entity.TireSize = "22";
            model.Entity.Price = 40;
        });
    }    
}
