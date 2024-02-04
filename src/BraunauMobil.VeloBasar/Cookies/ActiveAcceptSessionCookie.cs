using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Cookies;

public class ActiveAcceptSessionCookie
    : IActiveAcceptSessionCookie
{
    private readonly HttpContext _httpContext;

    public ActiveAcceptSessionCookie(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);

        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentException(nameof(httpContextAccessor.HttpContext));
    }

    public string Key { get; } = "activeAcceptSessionId";

    public CookieOptions Options { get; } = new()
    {
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromDays(2),
        SameSite = SameSiteMode.Strict,
        Secure = false
    };

    public void ClearActiveAcceptSession()
    {
        _httpContext.Response.Cookies.Delete(Key, Options);
    }

    public int? GetActiveAcceptSessionId()
    {
        string? id = _httpContext.Request.Cookies[Key];
        if (int.TryParse(id, out int sessionId))
        {
            return sessionId;
        }
        return null;
    }

    public void SetActiveAcceptSession(AcceptSessionEntity session)
    {
        ArgumentNullException.ThrowIfNull(session);

        _httpContext.Response.Cookies.Append(Key, session.Id.ToString(CultureInfo.InvariantCulture), Options);
    }
}
