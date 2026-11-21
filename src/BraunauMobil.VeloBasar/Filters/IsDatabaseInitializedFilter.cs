using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class IsDatabaseInitializedFilter(ILogger<IsDatabaseInitializedFilter> logger, IAppContext appContext, ISetupRouter router, IVeloCache veloCache)
    : AbstractVeloActionFilter
{
    protected override async Task ActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate nextDelegate)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nextDelegate);

        if (veloCache.IsDatabaseReady)
        {
            await nextDelegate();
        }
        else if (await appContext.NeedsInitialSetupAsync())
        {
            logger.LogInformation("Redirecting to initial setup");
            context.Result = new RedirectResult(router.ToInitialSetup());
        }
        else if (await appContext.NeedsMigrationAsync())
        {
            logger.LogInformation("Redirecting to migrate database");
            context.Result = new RedirectResult(router.ToMigrateDatabase());
        }
        else
        {
            veloCache.IsDatabaseReady = true;
            await nextDelegate();
        }
    }
}
