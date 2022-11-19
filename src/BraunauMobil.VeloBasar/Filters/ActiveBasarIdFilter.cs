using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class ActiveBasarEntityFilter
    : IAsyncActionFilter
{
    private const string _activeBasarIdArgumentName = "activeBasarId";

    private readonly IBasarService _basarService;
    private readonly IVeloRouter _router;
    private readonly VeloDbContext _db;

    public ActiveBasarEntityFilter(IBasarService basarService, IVeloRouter router, VeloDbContext db)
    {
        _basarService = basarService ?? throw new ArgumentNullException(nameof(basarService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        //  check if we need initial setup
        if (!_db.IsInitialized())
        {
            await next();
            return;
        }

        int? activeBasarId = await _basarService.GetActiveBasarIdAsync();

        if (context.ActionDescriptor.Parameters.Any(p => p.Name == _activeBasarIdArgumentName))
        {
            if (activeBasarId.HasValue)
            {
                context.ActionArguments[_activeBasarIdArgumentName] = activeBasarId;
            }
            else
            {
                await RedirectToBasarList(context);
                return;
            }
        }
        else
        {
            foreach (IActiveBasarModel activeBasarModel in context.ActionArguments.Values.OfType<IActiveBasarModel>())
            {
                if (activeBasarId.HasValue)
                {
                    activeBasarModel.ActiveBasarId = activeBasarId.Value;
                }
                else
                {
                    await RedirectToBasarList(context);
                    return;
                }
            }
        }

        await next();
    }

    private async Task RedirectToBasarList(ActionExecutingContext context)
    {
        if (context.Result != null)
        {
            await context.Result.ExecuteResultAsync(context);
        }

        context.Result = new RedirectResult(_router.Basar.ToList());
        return;
    }
}
