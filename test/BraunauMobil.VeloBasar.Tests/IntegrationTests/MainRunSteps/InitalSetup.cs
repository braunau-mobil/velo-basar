using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public class InitalSetup
    : TestStepBase
{
    private const string _adminUserEMail = "dev@shirenet.at";

    public InitalSetup(TestContext testContext)
        : base(testContext)
    { }

    public override async Task Run()
    {
        InitializationConfiguration configuration = Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        //  No Admin EMail
        configuration.AdminUserEMail = "";

        await Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.AdminUserEMail));
            view.Model.Should().Be(configuration);
        });

        AssertDb(db =>
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

        await Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.GenerateZipCodes));
            view.Model.Should().Be(configuration);
        });

        AssertDb(db =>
        {
            db.Users.Should().BeEmpty();
            db.Countries.Should().BeEmpty();
            db.ZipCodes.Should().BeEmpty();
            db.ProductTypes.Should().BeEmpty();
        });

        //  Valid config
        configuration.GenerateCountries = true;

        await Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=Index&controller=Home");
        });

        AssertDb(db =>
        {
            V.AdminUser = db.Users.Should().Contain(user => user.UserName == _adminUserEMail).Subject;

            V.Countries.Austria = db.Countries.AsNoTracking().Should().Contain(x => x.Name == "Österreich").Subject;
            V.Countries.Germany = db.Countries.AsNoTracking().Should().Contain(x => x.Name == "Deutschland").Subject;

            db.ZipCodes.AsNoTracking().Should().Contain(zipCode => zipCode.Zip == "5280");

            V.ProductTypes.Scooter = db.ProductTypes.AsNoTracking().Should().Contain(x => x.Name == "Scooter").Subject;
            V.ProductTypes.SteelSteed = db.ProductTypes.AsNoTracking().Should().Contain(x => x.Name == "Steel steed").Subject;
        });
    }
}
