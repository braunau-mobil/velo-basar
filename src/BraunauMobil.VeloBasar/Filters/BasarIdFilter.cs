using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Extensions;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class BasarIdFilter
    : IAsyncActionFilter
{
    public const string BasarIdArgumentName = "basarId";

    private readonly IBasarRouter _router;
    private readonly IBasarService _basarService;

    public BasarIdFilter(IBasarRouter router, IBasarService basarService)
    {
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _basarService = basarService ?? throw new ArgumentNullException(nameof(basarService));
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        int? activeBasarId = await _basarService.GetActiveBasarIdAsync();
        BasarEntity? activeBasar = null;
        if (activeBasarId.HasValue)
        {
            activeBasar = await _basarService.GetAsync(activeBasarId.Value);
        }

        if (context.ActionDescriptor.Parameters.Any(p => p.Name == BasarIdArgumentName))
        {
            if (activeBasar is not null)
            {
                if (!context.ActionArguments.ContainsKey(BasarIdArgumentName))
                {
                    context.ActionArguments[BasarIdArgumentName] = activeBasar.Id;
                }
            }
            else
            {
                await RedirectToBasarList(context);
                return;
            }
        }

        foreach (IHasBasarId activeBasarModel in context.ActionArguments.Values.OfType<IHasBasarId>())  
        {
            if (activeBasar is not null)
            {
                if (activeBasarModel.BasarId == 0)
                {
                    activeBasarModel.BasarId = activeBasar.Id;
                }
            }
            else
            {
                await RedirectToBasarList(context);
                return;
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

        context.Result = new RedirectResult(_router.ToList());
        return;
    }
}
