using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class CancelRouter
    : ControllerRouter
    , ICancelRouter
{
    public CancelRouter(LinkGenerator linkGenerator)
    : base(MvcHelper.ControllerName<CancelController>(), linkGenerator)
    { }

    public string ToSelectSale()
        => GetUriByAction(nameof(CancelController.SelectSale));

    public string ToSelectProducts(int saleId)
        => GetUriByAction(nameof(CancelController.SelectProducts), new { id = saleId });
}
