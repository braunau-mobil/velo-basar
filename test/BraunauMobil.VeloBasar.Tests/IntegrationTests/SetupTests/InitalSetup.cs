using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.SetupTests;

public class InitalSetup
    : TestBase
{
    [Fact]
    public async void GenerateAll()
    {
        //  Act
        InitializationConfiguration configuration = Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        configuration.AdminUserEMail = V.AdminUserEMail;
        configuration.GenerateCountries = true;
        configuration.GenerateProductTypes = true;
        configuration.GenerateZipCodes = true;

        await Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=Index&controller=Home");
        });

        //  Assert
        using VeloDbContext db = Services.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            db.Users.Should().Contain(user => user.UserName == V.AdminUserEMail);
            db.Countries.Should().Contain(country => country.Name == V.Countries.Austria);
            db.ZipCodes.Should().Contain(zipCode => zipCode.Zip == V.ZipCodes.Braunau);
            db.ProductTypes.Should().Contain(productType => productType.Name == V.ProductTypes.SteelSteed);
        }
    }

    [Fact]
    public async void GenerateNone()
    {
        //  Act
        InitializationConfiguration configuration = Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        configuration.AdminUserEMail = V.AdminUserEMail;
        configuration.GenerateCountries = false;
        configuration.GenerateProductTypes = false;
        configuration.GenerateZipCodes = false;

        await Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//action=Index&controller=Home");
        });

        //  Assert
        using VeloDbContext db = Services.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            db.Users.Should().Contain(user => user.UserName == V.AdminUserEMail);
            db.Countries.Should().BeEmpty();
            db.ZipCodes.Should().BeEmpty();
            db.ProductTypes.Should().BeEmpty();
        }
    }

    [Fact]
    public async void GenerateZipCodesIsCheckedButGenerateCountriesNot()
    {
        //  Act
        InitializationConfiguration configuration = Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        configuration.AdminUserEMail = V.AdminUserEMail;
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
    }

    [Fact]
    public async void NoAdminEMail()
    {
        //  Act
        InitializationConfiguration configuration = Do<SetupController, InitializationConfiguration>(controller =>
        {
            IActionResult result = controller.InitialSetup();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            return view.Model.Should().BeOfType<InitializationConfiguration>().Subject;
        });

        configuration.AdminUserEMail = "";
        configuration.GenerateCountries = false;
        configuration.GenerateProductTypes = false;
        configuration.GenerateZipCodes = false;

        await Do<SetupController>(async controller =>
        {
            IActionResult result = await controller.InitialSetupConfirmed(configuration);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(InitializationConfiguration.AdminUserEMail));
            view.Model.Should().Be(configuration);
        });
    }
}
