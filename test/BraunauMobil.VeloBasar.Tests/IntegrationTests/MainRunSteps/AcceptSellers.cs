using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public class AcceptSellers
    : TestStepBase
{
    public AcceptSellers(TestContext testContext)
        : base(testContext)
    { }

    public override async Task Run()
    {
        //  Create seller
        Do<AcceptSessionController>(controller =>
        {
            IActionResult result = controller.Start(V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=CreateForAcceptance&controller=Seller");
        });

        SellerCreateForAcceptanceModel createModel = await Do<SellerController, SellerCreateForAcceptanceModel>(async controller =>
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

        await Do<SellerController>(async controller =>
        {
            IActionResult result = await controller.CreateForAcceptance(createModel.Seller);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sellerId=1&action=StartForSeller&controller=AcceptSession");
        });

        AssertDb(db =>
        {
            SellerEntity frodo = db.Sellers.AsNoTracking().Should().Contain(s => s.FirstName == "Frodo" && s.LastName == "Beutlin").Subject;
            V.Sellers.Frodo = frodo;
        });

        //  Start accept session
        await Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.StartForSeller(V.Sellers.Frodo.Id, V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sessionId=1&action=Create&controller=AcceptProduct");
        });

        int acceptSessionId = 0;
        AssertDb(db =>
        {
            AcceptSessionEntity session = db.AcceptSessions.AsNoTracking().Should().Contain(s => s.SellerId == V.Sellers.Frodo.Id && s.BasarId == V.FirstBasar.Id).Subject;
            acceptSessionId = session.Id;
        });

        await EnterProducts(acceptSessionId);

        //  Finish accept session
        await Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.Submit(acceptSessionId);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//id=1&action=Success&controller=Transaction");
        });

        int acceptanceId = AssertDb(db =>
        {
            TransactionEntity transaction = db.AssertTransaction(V.FirstBasar.Id, TransactionType.Acceptance, 1);
            transaction.BasarId.Should().Be(V.FirstBasar.Id);
            transaction.CanCancel.Should().BeFalse();
            transaction.CanHasDocument.Should().BeTrue();
            transaction.Change.Should().BeNull();
            transaction.ChildTransactions.Should().BeEmpty();
            transaction.DocumentId.Should().BeNull();
            transaction.NeedsBankingQrCodeOnDocument.Should().BeFalse();
            transaction.NeedsStatusPush.Should().BeTrue();
            transaction.Notes.Should().BeNull();
            transaction.Number.Should().Be(1);
            transaction.ParentTransaction.Should().BeNull();
            transaction.TimeStamp.Should().Be(Context.Clock.GetCurrentDateTime());
            transaction.SellerId.Should().Be(V.Sellers.Frodo.Id);    
            transaction.Type.Should().Be(TransactionType.Acceptance);
            transaction.UpdateDocumentOnDemand.Should().BeTrue();

            transaction.Products.Should().HaveCount(2);
            transaction.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Stahlross.Id);
            transaction.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Einrad.Id);

            V.Products.FirstBasar.AssertStorageStates(db, StorageState.Available, StorageState.Available);
            V.Products.FirstBasar.AssertValueStates(db, ValueState.NotSettled, ValueState.NotSettled);

            db.Files.AsNoTracking().Should().BeEmpty();

            return transaction.Id;
        });

        

        await Do<TransactionController>(async controller =>
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

        AssertDb(db =>
        {
            db.Files.AsNoTracking().Should().BeEmpty();
        });

        //  Download document
        await Do<TransactionController>(async controller =>
        {
            IActionResult result = await controller.Document(acceptanceId);

            FileContentResult file = result.Should().BeOfType<FileContentResult>().Subject;
            file.ContentType.Should().Be("application/pdf");
            file.FileDownloadName.Should().Be("2063-04-05T11:22:33_Acceptance-1.pdf");
            file.FileContents.Should().NotBeNull();
        });

        AssertDb(db =>
        {
            TransactionEntity acceptance = db.Transactions.AsNoTracking().Should().Contain(f => f.Id == acceptanceId).Subject;

            acceptance.DocumentId.Should().NotBeNull();
        });
    }
    private async Task EnterProducts(int acceptSessionId)
    {
        V.Products.FirstBasar.Frodo.Stahlross = await EnterProduct(acceptSessionId, model =>
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

        V.Products.FirstBasar.Frodo.Einrad = await EnterProduct(acceptSessionId, model =>
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
