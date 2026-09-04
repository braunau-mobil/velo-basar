using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public class CountryRouter
    : CrudRouter<CountryEntity>
    , ICountryRouter
{
    public CountryRouter(LinkGenerator linkGenerator)
        : base(linkGenerator)
    { }
}
