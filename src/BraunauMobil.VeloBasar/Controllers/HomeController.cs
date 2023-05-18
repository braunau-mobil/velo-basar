using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class HomeController
    : AbstractVeloController
{
    private readonly IAppContext _appContext;
    private readonly ILogger<HomeController> _logger;
    private readonly IVeloRouter _router;
    
    private readonly ICurrentThemeCookie _cookie;

    public HomeController(IAppContext appContext, IVeloRouter router, ILogger<HomeController> logger, ICurrentThemeCookie cookie)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _cookie = cookie ?? throw new ArgumentNullException(nameof(cookie));
    }

    public IActionResult Index()
    {
        //  check if we need initial setup
        if (!_appContext.IsDatabaseInitialized())
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
