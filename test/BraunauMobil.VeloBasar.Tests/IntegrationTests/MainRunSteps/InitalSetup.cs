using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public static class InitalSetup
{
    public static async Task Run(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        InitializationConfiguration configuration = services.Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        //  No Admin EMail
        configuration.AdminUserEMail = "";
        configuration.GenerateCountries = false;
        configuration.GenerateProductTypes = false;
        configuration.GenerateZipCodes = false;

        await services.Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.AdminUserEMail));
            view.Model.Should().Be(configuration);
        });

        services.AssertDb(db =>
        {
            db.Users.Should().BeEmpty();
            db.Countries.Should().BeEmpty();
            db.ZipCodes.Should().BeEmpty();
            db.ProductTypes.Should().BeEmpty();
        });

        //  Invalid Generation config
        configuration.AdminUserEMail = V.AdminUserEMail;
        configuration.GenerateCountries = false;
        configuration.GenerateProductTypes = true;
        configuration.GenerateZipCodes = true;

        await services.Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.GenerateZipCodes));
            view.Model.Should().Be(configuration);
        });

        services.AssertDb(db =>
        {
            db.Users.Should().BeEmpty();
            db.Countries.Should().BeEmpty();
            db.ZipCodes.Should().BeEmpty();
            db.ProductTypes.Should().BeEmpty();
        });

        //  Valid config
        configuration.AdminUserEMail = V.AdminUserEMail;
        configuration.GenerateCountries = true;
        configuration.GenerateProductTypes = true;
        configuration.GenerateZipCodes = true;

        await services.Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=Index&controller=Home");
        });

        services.AssertDb(db =>
        {
            db.Users.Should().Contain(user => user.UserName == V.AdminUserEMail);
            db.Countries.Should().Contain(country => country.Name == V.Countries.Austria);
            db.ZipCodes.Should().Contain(zipCode => zipCode.Zip == V.ZipCodes.Braunau);
            db.ProductTypes.Should().Contain(productType => productType.Name == V.ProductTypes.SteelSteed);
        });
    }
}
