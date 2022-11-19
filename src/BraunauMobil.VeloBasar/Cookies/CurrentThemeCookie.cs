using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Http;

namespace BraunauMobil.VeloBasar.Cookies;

public static class CurrentThemeCookie
{
    private static readonly Cookie _currentThemeId = new(
        "currentTheme",
        new CookieOptions
        {
            IsEssential = true,
            MaxAge = TimeSpan.FromDays(2)
        });

    public static Theme GetCurrentTheme(this IRequestCookieCollection cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        string? currentThemeString = cookies[_currentThemeId.Key];
        if (Enum.TryParse<Theme>(currentThemeString, out Theme theme))
        {
            return theme;
        }
        return Theme.DefaultLight;
    }
    public static void SetCurrentTheme(this IResponseCookies cookies, Theme theme)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        cookies.Append(_currentThemeId.Key, $"{theme}");
    }
}
