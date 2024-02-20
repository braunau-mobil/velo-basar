using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Filters;

public sealed class ActiveSessionIdFilter(IActiveAcceptSessionCookie cookie, VeloDbContext db)
    : AbstractVeloActionFilter
{
    protected override async Task ActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate nextDelegate)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nextDelegate);

        int? activeSessionId = cookie.GetActiveAcceptSessionId();
        if (activeSessionId.HasValue)
        {
            AcceptSessionEntity? acceptSession = await db.AcceptSessions.AsNoTracking().FirstOrDefaultByIdAsync(activeSessionId.Value);
            if (acceptSession != null && !acceptSession.IsCompleted && context.Controller is Controller controller)
            {
                controller.ViewData.SetActiveSessionId(activeSessionId.Value);
            }
            else
            {
                cookie.ClearActiveAcceptSession();
            }
        }

        await nextDelegate();
    }
}
