using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class DevController
    : AbstractVeloController
{
    private readonly IAppContext _appContext;
    private readonly IDataGeneratorService _generatorService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IVeloRouter _router;

    public DevController(IAppContext appContext, IDataGeneratorService dataGeneratorService, UserManager<IdentityUser> userManager, IVeloRouter router)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _generatorService = dataGeneratorService ?? throw new ArgumentNullException(nameof(dataGeneratorService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _router = router ?? throw new ArgumentNullException(nameof(router));
    }

    public IActionResult DeleteCookies()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        return View();
    }

    [HttpPost]
    [ActionName(nameof(DeleteCookies))]
    public IActionResult DeleteCookiesConfirmed()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        foreach (string cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        return Redirect(_router.ToHome());
    }

    public IActionResult DropDatabase()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        return View();
    }
    
    [HttpPost]
    [ActionName(nameof(DropDatabase))]
    public async Task<IActionResult> DropDatabaseConfirmed()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        await _generatorService.DropDatabaseAsync();

        return Redirect(_router.ToHome());
    }

    public async Task<IActionResult> UnlockAllUsers()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }

        foreach (IdentityUser user in await _userManager.Users.ToListAsync())
        {
            await _userManager.SetLockoutEnabledAsync(user, false);
        }

        return Redirect(_router.ToHome());
    }
}
