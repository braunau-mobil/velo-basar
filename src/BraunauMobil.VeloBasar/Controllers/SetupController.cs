using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class SetupController
    : AbstractVeloController
{
    private readonly ISetupService _setupService;
    private readonly IVeloRouter _router;
    private readonly IValidator<InitializationConfiguration> _validator;

    public SetupController(ISetupService setupService, IVeloRouter router, IValidator<InitializationConfiguration> validator)
    {
        _setupService = setupService ?? throw new ArgumentNullException(nameof(setupService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public IActionResult InitialSetup()
        => View(new InitializationConfiguration());

    [HttpPost, ActionName(nameof(InitialSetup))]
    public async Task<IActionResult> InitialSetupConfirmed(InitializationConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        foreach (string cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        SetValidationResult(await _validator.ValidateAsync(config));

        if (ModelState.IsValid)
        {
            await _setupService.CreateDatabaseAsync();
            await _setupService.InitializeDatabaseAsync(config);

            return Redirect(_router.ToHome());
        }

        return View(config);
    }
}
