using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Extensions;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class ActiveBasarEntityFilter
    : IAsyncActionFilter
{
    private const string _activeBasarIdArgumentName = "activeBasarId";

    private readonly IVeloRouter _router;
    private readonly VeloDbContext _db;

    public ActiveBasarEntityFilter(IVeloRouter router, VeloDbContext db)
    {
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

        BasarEntity? activeBasar = await _db.Basars.AsNoTracking()
            .Where(b => b.State == ObjectState.Enabled)
            .FirstOrDefaultAsync();

        if (context.ActionDescriptor.Parameters.Any(p => p.Name == _activeBasarIdArgumentName))
        {
            if (activeBasar is not null)
            {
                context.ActionArguments[_activeBasarIdArgumentName] = activeBasar.Id;
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
                if (activeBasar is not null)
                {
                    activeBasarModel.ActiveBasarId = activeBasar.Id;
                }
                else
                {
                    await RedirectToBasarList(context);
                    return;
                }
            }
        }

        if (context.Controller is Controller controller)
        {
            controller.ViewData.SetActiveBasar(activeBasar);
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
