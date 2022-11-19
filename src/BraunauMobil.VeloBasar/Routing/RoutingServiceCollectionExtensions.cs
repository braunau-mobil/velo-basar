using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Routing;

public static class RoutingServiceCollectionExtensions
{
    public static IServiceCollection AddVeloRouting(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddSingleton<IAcceptProductRouter, AcceptProductsRouter>()
            .AddSingleton<IAcceptSessionRouter, AcceptSessionRouter>()
            .AddSingleton<IAdminRouter, AdminRouter>()
            .AddSingleton<ICancelRouter, CancelRouter>()
            .AddSingleton<ICartRouter, CartRouter>()
            .AddSingleton<IDevRouter, DevRouter>()
            .AddSingleton<IAcceptanceLabelsRouter, AcceptanceLabelsRouter>()
            .AddSingleton<IProductRouter, ProductRouter>()
            .AddSingleton<ISellerRouter, SellerRouter>()
            .AddSingleton<ISetupRouter, SetupRouter>()
            .AddSingleton<ITransactionRouter, TransactionRouter>()
            .AddSingleton<IVeloRouter, VeloRouter>()
            ;
    }
}
