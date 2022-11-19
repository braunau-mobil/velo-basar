using Microsoft.AspNetCore.Http;

namespace BraunauMobil.VeloBasar.Cookies;

public static class BasarIdCookie
{
    private static readonly Cookie _basarId = new(
        "basarId",
        new CookieOptions
        {
            IsEssential = true,
            MaxAge = TimeSpan.FromDays(2)
        });

    public static int? GetBasarId(this IRequestCookieCollection cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        string? id = cookies[_basarId.Key];
        if (int.TryParse(id, out int basarId))
        {
            return basarId;
        }
        return null;
    }
    public static void SetBasarId(this IResponseCookies cookies, int basarId)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        cookies.Append(_basarId.Key, $"{basarId}");
    }
}
