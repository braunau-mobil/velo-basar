using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class SecurityController
    : AbstractVeloController
{
    private readonly ILogger<SecurityController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IVeloRouter _router;
    private readonly IValidator<LoginModel> _loginValidator;

    public SecurityController(SignInManager<IdentityUser> signInManager, IVeloRouter router, ILogger<SecurityController> logger, IValidator<LoginModel> validator)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _loginValidator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IActionResult> Login()
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View(new LoginModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        SetValidationResult(await _loginValidator.ValidateAsync(model));

        if (ModelState.IsValid)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Redirect(_router.ToHome());
            }

            model.ShowErrorMessage = true;
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return Redirect(_router.ToHome());
    }
}
