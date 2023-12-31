using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Routing;

public static class RoutingServiceCollectionExtensions
{
    public static IServiceCollection AddVeloRouting(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddSingleton<IAcceptProductRouter, AcceptProductRouter>()
            .AddSingleton<IAcceptSessionRouter, AcceptSessionRouter>()
            .AddSingleton<IAdminRouter, AdminRouter>()
            .AddSingleton<IBasarRouter, BasarRouter>()
            .AddSingleton<ICancelRouter, CancelRouter>()
            .AddSingleton<ICartRouter, CartRouter>()
            .AddSingleton<ICountryRouter, CountryRouter>()
            .AddSingleton<IDevRouter, DevRouter>()
            .AddSingleton<IAcceptanceLabelsRouter, AcceptanceLabelsRouter>()
            .AddSingleton<IProductRouter, ProductRouter>()
            .AddSingleton<IProductTypeRouter, ProductTypeRouter>()
            .AddSingleton<ISellerRouter, SellerRouter>()
            .AddSingleton<ISetupRouter, SetupRouter>()
            .AddSingleton<ITransactionRouter, TransactionRouter>()
            .AddSingleton<IVeloRouter, VeloRouter>()
            ;
    }
}
