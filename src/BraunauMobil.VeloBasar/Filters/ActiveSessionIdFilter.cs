using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class ActiveSessionIdFilter
    : IAsyncActionFilter
{
    private readonly IActiveAcceptSessionCookie _cookie;
    private readonly VeloDbContext _db;

    public ActiveSessionIdFilter(IActiveAcceptSessionCookie cookie, VeloDbContext db)
    {
        _cookie = cookie ?? throw new ArgumentNullException(nameof(cookie));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        int? activeSessionId = _cookie.GetActiveAcceptSessionId();
        if (activeSessionId.HasValue)
        {
            AcceptSessionEntity? acceptSession = await _db.AcceptSessions.AsNoTracking().FirstOrDefaultByIdAsync(activeSessionId.Value);
            if (acceptSession != null && !acceptSession.IsCompleted && context.Controller is Controller controller)
            {
                controller.ViewData.SetActiveSessionId(activeSessionId.Value);
            }
            else
            {
                _cookie.ClearActiveAcceptSession();
            }
        }

        await next();
    }
}
