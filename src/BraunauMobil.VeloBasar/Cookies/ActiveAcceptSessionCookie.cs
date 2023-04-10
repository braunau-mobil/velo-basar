using Microsoft.AspNetCore.Http;
using Xan.AspNetCore.Http;

namespace BraunauMobil.VeloBasar.Cookies;

public static class ActiveAcceptSessionCookie
{
    public static readonly CookieConfig ActiveAcceptSession = new(
        "activeAcceptSessionId",
        new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            MaxAge = TimeSpan.FromDays(2),
            SameSite = SameSiteMode.Strict,
            Secure = false
        });

    public static void ClearActiveAcceptSession(this IResponseCookies cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        cookies.Delete(ActiveAcceptSession.Key, ActiveAcceptSession.CookieOptions);
    }

    public static int? GetActiveAcceptSessionId(this IRequestCookieCollection cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        string? id = cookies[ActiveAcceptSession.Key];
        if (int.TryParse(id, out int sessionId))
        {
            return sessionId;
        }
        return null;
    }

    public static void SetActiveAcceptSession(this IResponseCookies cookies, AcceptSessionEntity session)
    {
        ArgumentNullException.ThrowIfNull(cookies);
        ArgumentNullException.ThrowIfNull(session);

        cookies.Append(ActiveAcceptSession.Key, $"{session.Id}", ActiveAcceptSession.CookieOptions);
    }
}
