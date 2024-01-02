using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Rendering;

public static class RenderingServiceCollectionExtensions
{
    public static IServiceCollection AddVeloRendering(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddScoped<IVeloHtmlFactory, DefaultVeloHtmlFactory>()
            ;
    }
}
