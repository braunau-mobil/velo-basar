using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class HomeController(IVeloRouter router, ICurrentThemeCookie cookie)
    : AbstractVeloController
{
    private readonly IVeloRouter _router = router ?? throw new ArgumentNullException(nameof(router));
    private readonly ICurrentThemeCookie _cookie = cookie ?? throw new ArgumentNullException(nameof(cookie));

    public IActionResult Index()
        => Redirect(_router.Basar.ToActiveBasarDetails());

    public IActionResult SetTheme(Theme theme)
    {
        _cookie.SetCurrentTheme(theme);

        return RedirectToReferer();
    }
}
