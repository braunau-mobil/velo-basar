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
            
            acceptance.Products.Should().HaveCount(1);
            ProductToTransactionEntity stahlrossToAcceptance = acceptance.Products.Should().Contain(p => p.ProductId == V.Products.FirstBasar.Frodo.Stahlross.Id).Subject;
            ProductEntity stahlross = db.Products.AsNoTracking().Should().Contain(p => p.Id == stahlrossToAcceptance.ProductId).Subject;
            stahlross.StorageState.Should().Be(StorageState.Available);
            stahlross.ValueState.Should().Be(ValueState.NotSettled);

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
        await AcceptStahlross(context, acceptSessionId);
    }

    private static async Task AcceptStahlross(TestContext context, int acceptSessionId)
    {
        AcceptProductModel stahlross = await context.Do<AcceptProductController, AcceptProductModel>(async controller =>
        {
            IActionResult result = await controller.Create(acceptSessionId);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CreateEdit");
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            return view.Model.Should().BeOfType<AcceptProductModel>().Subject;
        });

        stahlross.Entity.TypeId = V.ProductTypes.Stahlross.Id;
        stahlross.Entity.Brand = "Simplon";
        stahlross.Entity.Color = "Schwarz";
        stahlross.Entity.FrameNumber = "1234567890";
        stahlross.Entity.Description = "Gepäckträger, Korb";
        stahlross.Entity.TireSize = "26";
        stahlross.Entity.Price = 120;

        await context.Do<AcceptProductController>(async controller =>
        {
            IActionResult result = await controller.Create(stahlross.Entity);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sessionId=1&action=Create&controller=AcceptProduct");
        });

        context.AssertDb(db =>
        {
            V.Products.FirstBasar.Frodo.Stahlross = db.Products.AsNoTracking().Should().Contain(p => p.SessionId == acceptSessionId && p.Price == 120).Subject;

            V.Products.FirstBasar.Frodo.Stahlross.Brand.Should().Be("Simplon");
            V.Products.FirstBasar.Frodo.Stahlross.Color.Should().Be("Schwarz");
            V.Products.FirstBasar.Frodo.Stahlross.FrameNumber.Should().Be("1234567890");
            V.Products.FirstBasar.Frodo.Stahlross.Description.Should().Be("Gepäckträger, Korb");
            V.Products.FirstBasar.Frodo.Stahlross.TireSize.Should().Be("26");
            V.Products.FirstBasar.Frodo.Stahlross.Price.Should().Be(120);
        });
    }
}
