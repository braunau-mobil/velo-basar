using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BraunauMobil.VeloBasar.Cookies;

public class CurrentThemeCookie
    : ICurrentThemeCookie
{
    private readonly HttpContext _httpContext;

    public CurrentThemeCookie(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);

        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentException(nameof(httpContextAccessor.HttpContext));
    }

    public string Key { get; } = "currentTheme";
    
    public CookieOptions Options { get; } = new()
    {
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromDays(2),
        SameSite = SameSiteMode.Strict,
        Secure = false
    };

    public Theme GetCurrentTheme()
    {
        string? currentThemeString = _httpContext.Request.Cookies[Key];
        if (Enum.TryParse<Theme>(currentThemeString, out Theme theme))
        {
            return theme;
        }
        return Theme.DefaultLight;
    }

    public void SetCurrentTheme(Theme theme)
    {
        _httpContext.Response.Cookies.Append(Key, theme.ToString(), Options);
    }
}
