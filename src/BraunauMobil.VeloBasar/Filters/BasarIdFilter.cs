using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Extensions;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class BasarIdFilter(IBasarRouter router, IBasarService basarService)
    : AbstractVeloActionFilter
{
    public const string BasarIdArgumentName = "basarId";

    protected override async Task ActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate nextDelegate)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nextDelegate);

        int? activeBasarId = await basarService.GetActiveBasarIdAsync();
        BasarEntity? activeBasar = null;
        if (activeBasarId.HasValue)
        {
            activeBasar = await basarService.GetAsync(activeBasarId.Value);
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

        await nextDelegate();
    }

    private async Task RedirectToBasarList(ActionExecutingContext context)
    {
        if (context.Result != null)
        {
            await context.Result.ExecuteResultAsync(context);
        }

        context.Result = new RedirectResult(router.ToList());
        return;
    }
}
