using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class HomeController
    : AbstractVeloController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IVeloRouter _router;
    private readonly VeloDbContext _db;

    public HomeController(IVeloRouter router, ILogger<HomeController> logger, VeloDbContext db)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public IActionResult Index()
    {
        //  check if we need initial setup
        if (!_db.IsInitialized())
        {
            _logger.LogInformation("DB not initialized");
            return Redirect(_router.Setup.ToInitialSetup());
        }

        return Redirect(_router.Basar.ToActiveBasarDetails());
    }

    public IActionResult SetTheme(Theme theme)
    {
        Response.Cookies.SetCurrentTheme(theme);

        return RedirectToReferer();
    }
}
