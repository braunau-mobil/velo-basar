using System.Diagnostics;

namespace BraunauMobil.VeloBasar.Rendering;

public enum Theme
{
    DefaultLight,
    DefaultDark,
    Brutal
}

public static class ThemeExtensions
{
    public static string CssFilePath(this Theme theme)
        => theme switch
        {
            Theme.DefaultLight => "/css/defaul_-light.css",
            Theme.DefaultDark => "/css/default_dark.css",
            Theme.Brutal => "/css/neo_brutal.css",
            _ => throw new UnreachableException()
        };

}
