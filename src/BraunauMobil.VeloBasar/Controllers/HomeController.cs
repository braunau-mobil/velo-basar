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
    private readonly ICurrentThemeCookie _cookie;

    public HomeController(IVeloRouter router, ILogger<HomeController> logger, VeloDbContext db, ICurrentThemeCookie cookie)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _cookie = cookie ?? throw new ArgumentNullException(nameof(cookie));
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
        _cookie.SetCurrentTheme(theme);

        return RedirectToReferer();
    }
}
