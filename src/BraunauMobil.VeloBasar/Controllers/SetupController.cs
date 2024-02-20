using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class SetupController(ISetupService setupService, IVeloRouter router, IValidator<InitializationConfiguration> validator)
    : AbstractVeloController
{
    [SkipVeloFilters]
    public IActionResult InitialSetup()
        => View(new InitializationConfiguration());

    [HttpPost, ActionName(nameof(InitialSetup)), SkipVeloFilters]
    public async Task<IActionResult> InitialSetupConfirmed(InitializationConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        foreach (string cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        SetValidationResult(await validator.ValidateAsync(config));

        if (ModelState.IsValid)
        {
            await setupService.MigrateDatabaseAsync();
            await setupService.InitializeDatabaseAsync(config);

            return Redirect(router.ToHome());
        }

        return View(config);
    }

    [SkipVeloFilters]
    public IActionResult MigrateDatabase()
        => View();

    [HttpPost, ActionName(nameof(MigrateDatabase)), SkipVeloFilters]
    public async Task<IActionResult> MigrateDatabaseConfirmed()
    {
        await setupService.MigrateDatabaseAsync();
        
        return Redirect(router.ToHome());
    }
}
