using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class DevController(IAppContext appContext, IDataGeneratorService generatorService, UserManager<IdentityUser> userManager, IVeloRouter router)
    : AbstractVeloController
{
    public IActionResult DeleteCookies()
    {
        if (!appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        return View();
    }

    [HttpPost]
    [ActionName(nameof(DeleteCookies))]
    public IActionResult DeleteCookiesConfirmed()
    {
        if (!appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        foreach (string cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        return Redirect(router.ToHome());
    }

    public IActionResult DropDatabase()
    {
        if (!appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        return View();
    }
    
    [HttpPost]
    [ActionName(nameof(DropDatabase))]
    public async Task<IActionResult> DropDatabaseConfirmed()
    {
        if (!appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        await generatorService.DropDatabaseAsync();
        await generatorService.CreateDatabaseAsync();

        return Redirect(router.ToHome());
    }

    public async Task<IActionResult> UnlockAllUsers()
    {
        if (!appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }

        foreach (IdentityUser user in await userManager.Users.ToListAsync())
        {
            await userManager.SetLockoutEnabledAsync(user, false);
        }

        return Redirect(router.ToHome());
    }
}
