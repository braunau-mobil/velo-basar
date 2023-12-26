using BraunauMobil.VeloBasar.Rendering;

namespace BraunauMobil.VeloBasar.Routing;

public interface IVeloRouter
{
    IAcceptanceLabelsRouter AcceptanceLabels { get; }

    IAcceptProductRouter AcceptProduct { get; }

    IAcceptSessionRouter AcceptSession { get; }

    IAdminRouter Admin { get; }

    IBasarRouter Basar { get; }

    ICancelRouter Cancel { get; }

    ICartRouter Cart { get; }

    ICountryRouter Country { get; }

    IDevRouter Dev { get; }    

    IProductRouter Product { get; }

    IProductTypeRouter ProductType { get; }

    ISellerRouter Seller { get; }

    ISetupRouter Setup { get; }

    ITransactionRouter Transaction { get; }

    string ToHome();

    string ToLogin();

    string ToLogout();

    string ToSetTheme(Theme theme);
}
