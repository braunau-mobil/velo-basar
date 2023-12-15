using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public static class InitalSetup
{
    private const string _adminUserEMail = "dev@shirenet.at";

    public static async Task Run(TestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        InitializationConfiguration configuration = context.Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        //  No Admin EMail
        configuration.AdminUserEMail = "";

        await context.Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.AdminUserEMail));
            view.Model.Should().Be(configuration);
        });

        context.AssertDb(db =>
        {
            db.Users.Should().BeEmpty();
            db.Countries.Should().BeEmpty();
            db.ZipCodes.Should().BeEmpty();
            db.ProductTypes.Should().BeEmpty();
        });

        //  Invalid Generation config
        configuration.AdminUserEMail = _adminUserEMail;
        configuration.GenerateCountries = false;
        configuration.GenerateProductTypes = true;
        configuration.GenerateZipCodes = true;

        await context.Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.GenerateZipCodes));
            view.Model.Should().Be(configuration);
        });

        context.AssertDb(db =>
        {
            db.Users.Should().BeEmpty();
            db.Countries.Should().BeEmpty();
            db.ZipCodes.Should().BeEmpty();
            db.ProductTypes.Should().BeEmpty();
        });

        //  Valid config
        configuration.GenerateCountries = true;

        await context.Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=Index&controller=Home");
        });

        context.AssertDb(db =>
        {
            V.AdminUser = db.Users.Should().Contain(user => user.UserName == _adminUserEMail).Subject;

            V.Countries.Austria = db.Countries.AsNoTracking().Should().Contain(x => x.Name == "Österreich").Subject;
            V.Countries.Germany = db.Countries.AsNoTracking().Should().Contain(x => x.Name == "Deutschland").Subject;

            db.ZipCodes.AsNoTracking().Should().Contain(zipCode => zipCode.Zip == "5280");

            V.ProductTypes.Stahlross = db.ProductTypes.AsNoTracking().Should().Contain(x => x.Name == "Stahlross").Subject;
        });
    }
}
