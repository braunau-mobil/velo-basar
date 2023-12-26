using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public class ProductTypeRouter
    : CrudRouter<ProductTypeEntity>
    , IProductTypeRouter
{
    public ProductTypeRouter(LinkGenerator linkGenerator)
        : base(linkGenerator)
    { }
}
