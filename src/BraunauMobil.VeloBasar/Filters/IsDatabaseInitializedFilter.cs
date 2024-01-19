using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class IsDatabaseInitializedFilter
    : IAsyncActionFilter
{
    private readonly ILogger<IsDatabaseInitializedFilter> _logger;
    private readonly IAppContext _appContext;
    private readonly IVeloRouter _router;

    public IsDatabaseInitializedFilter(IAppContext appContext, IVeloRouter router, ILogger<IsDatabaseInitializedFilter> logger)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        //  check if we need initial setup
        if (!_appContext.IsDatabaseInitialized())
        {
            _logger.LogInformation("DB not initialized");

            if (context.Result != null)
            {
                await context.Result.ExecuteResultAsync(context);
            }
            context.Result = new RedirectResult(_router.Setup.ToInitialSetup());
        }
        else
        {
            await next();
        }
    }
}
