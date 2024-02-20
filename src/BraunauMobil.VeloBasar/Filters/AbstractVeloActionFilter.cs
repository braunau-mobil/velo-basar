using Microsoft.AspNetCore.Mvc.Filters;

namespace BraunauMobil.VeloBasar.Filters;

public abstract class AbstractVeloActionFilter
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        if (context.ActionDescriptor.EndpointMetadata.OfType<SkipVeloFiltersAttribute>().Any())
        {
            await next();
            return;
        }

        await ActionExecutionAsync(context, next);
    }

    protected abstract Task ActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate nextDelegate);
}
