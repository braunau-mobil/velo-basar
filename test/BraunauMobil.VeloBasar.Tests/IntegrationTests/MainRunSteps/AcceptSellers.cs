using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public static class AcceptSellers
{
    public static async Task Run(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        //  Create seller
        await services.Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.Start(V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=CreateForAcceptance&controller=Seller");
        });

        SellerCreateForAcceptanceModel createModel = await services.Do<SellerController, SellerCreateForAcceptanceModel>(async controller =>
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

        await services.Do<SellerController>(async controller =>
        {
            IActionResult result = await controller.CreateForAcceptance(createModel.Seller);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sellerId=1&action=StartForSeller&controller=AcceptSession");
        });

        services.AssertDb(db =>
        {
            SellerEntity frodo = db.Sellers.AsNoTracking().Should().Contain(s => s.FirstName == "Frodo" && s.LastName == "Beutlin").Subject;
            V.Sellers.Frodo = frodo;
        });

        //  Start accept session
        await services.Do<AcceptSessionController>(async controller =>
        {
            IActionResult result = await controller.StartForSeller(V.Sellers.Frodo.Id, V.FirstBasar.Id);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sessionId=1&action=Create&controller=AcceptProduct");
        });

        int acceptSessionId = 0;
        services.AssertDb(db =>
        {
            AcceptSessionEntity session = db.AcceptSessions.AsNoTracking().Should().Contain(s => s.SellerId == V.Sellers.Frodo.Id && s.BasarId == V.FirstBasar.Id).Subject;
            acceptSessionId = session.Id;
        });

        await AcceptProducts(services, acceptSessionId);
    }
    private static async Task AcceptProducts(IServiceProvider services, int acceptSessionId)
    {
        await AcceptStahlross(services, acceptSessionId);
    }

    private static async Task AcceptStahlross(IServiceProvider services, int acceptSessionId)
    {
        AcceptProductModel stahlross = await services.Do<AcceptProductController, AcceptProductModel>(async controller =>
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

        await services.Do<AcceptProductController>(async controller =>
        {
            IActionResult result = await controller.Create(stahlross.Entity);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//sessionId=1&action=Create&controller=AcceptProduct");
        });

        services.AssertDb(db =>
        {
            V.Products.FirstBasar.Frodo.Stahlross = db.Products.AsNoTracking().Should().Contain(p => p.SessionId == acceptSessionId && p.Price == 120).Subject;
        });
    }
}
