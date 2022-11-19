using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class DevController
    : AbstractVeloController
{
    private readonly VeloDbContext _db;
    private readonly IAppContext _appContext;
    private readonly IDataGeneratorService _generatorService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IVeloRouter _router;

    public DevController(VeloDbContext db, IAppContext appContext, IDataGeneratorService dataGeneratorService, UserManager<IdentityUser> userManager, IVeloRouter router)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _generatorService = dataGeneratorService ?? throw new ArgumentNullException(nameof(dataGeneratorService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _router = router ?? throw new ArgumentNullException(nameof(router));
    }

    public IActionResult DangerZone()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        DataGeneratorConfiguration config = new()
        {
            GenerateBrands = true,
            GenerateCountries = true,
            GenerateProductTypes = true,
            GenerateZipCodes = true
        };
        return View(config);
    }

    [HttpPost]
    public async Task<IActionResult> DangerZone(DataGeneratorConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        _generatorService.Contextualize(configuration);
        await _generatorService.GenerateAsync();
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
    public async Task<IActionResult> DropDatabaseConfirment()
    {
        if (!_appContext.DevToolsEnabled())
        {
            return Unauthorized();
        }
        await _db.Database.EnsureDeletedAsync();
        await _db.SaveChangesAsync();

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
