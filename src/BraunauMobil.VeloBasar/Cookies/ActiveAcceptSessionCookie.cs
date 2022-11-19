using Microsoft.AspNetCore.Http;

namespace BraunauMobil.VeloBasar.Cookies;

public static class ActiveAcceptSessionCooke
{
    private static readonly Cookie _activeAcceptSession = new(
        "activeAcceptSessionId",
        new CookieOptions
        {
            IsEssential = true,
            MaxAge = TimeSpan.FromDays(2)
        });

    public static void ClearActiveAcceptSession(this IResponseCookies cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        cookies.Delete(_activeAcceptSession.Key);
    }

    public static int? GetActiveAcceptSessionId(this IRequestCookieCollection cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        string? id = cookies[_activeAcceptSession.Key];
        if (int.TryParse(id, out int basarId))
        {
            return basarId;
        }
        return null;
    }

    public static void SetActiveAcceptSession(this IResponseCookies cookies, AcceptSessionEntity session)
    {
        ArgumentNullException.ThrowIfNull(cookies);
        ArgumentNullException.ThrowIfNull(session);

        cookies.Append(_activeAcceptSession.Key, $"{session.Id}");
    }
}
