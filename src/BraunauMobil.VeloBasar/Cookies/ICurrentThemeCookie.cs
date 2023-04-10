using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Cookies;

public interface ICurrentThemeCookie
    : ICookie
{
    Theme GetCurrentTheme();

    void SetCurrentTheme(Theme theme);
}
