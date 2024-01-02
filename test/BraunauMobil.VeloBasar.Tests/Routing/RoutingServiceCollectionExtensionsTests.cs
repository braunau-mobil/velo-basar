using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.Routing;

public class RoutingServiceCollectionExtensionsTests
{
    [Fact]
    public void AllServicesShouldbeRegistered()
    {
        //  Arrange
        IServiceCollection services = new ServiceCollection();

        //  Act
        services.AddVeloRouting();

        //  Assert
        services.Should().SatisfyRespectively(
            X.CreateInspector<IAcceptProductRouter, AcceptProductRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IAcceptSessionRouter, AcceptSessionRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IAdminRouter, AdminRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IBasarRouter, BasarRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<ICancelRouter, CancelRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<ICartRouter, CartRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<ICountryRouter, CountryRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IDevRouter, DevRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IAcceptanceLabelsRouter, AcceptanceLabelsRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IProductRouter, ProductRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IProductTypeRouter, ProductTypeRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<ISellerRouter, SellerRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<ISetupRouter, SetupRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<ITransactionRouter, TransactionRouter>(ServiceLifetime.Singleton),
            X.CreateInspector<IVeloRouter, VeloRouter>(ServiceLifetime.Singleton)
            );
    }        
}
