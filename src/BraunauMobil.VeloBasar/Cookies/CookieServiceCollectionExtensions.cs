using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Cookies;

public static class CookieServiceCollectionExtensions
{
    public static IServiceCollection AddVeloCookies(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddScoped<IActiveAcceptSessionCookie, ActiveAcceptSessionCookie>()
            .AddScoped<ICartCookie, CartCookie>()
            .AddScoped<ICurrentThemeCookie, CurrentThemeCookie>()
            ;
    }
}
