using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public class CountryRouter
    : CrudRouter<CountryEntity>
    , ICountryRouter
{
    public CountryRouter(LinkGenerator linkGenerator)
        : base(linkGenerator)
    { }
}
