using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class AcceptProductsRouter
    : ControllerRouter
    , IAcceptProductRouter
{
    public AcceptProductsRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<AcceptProductController>(), linkGenerator)
    { }

    public string ToCreate(int sessionId)
        => GetUriByAction(nameof(AcceptProductController.Create), new { sessionId });

    public string ToDelete(int sessionId, int productId)
        => GetUriByAction(nameof(AcceptProductController.Delete), new { sessionId, productId });

    public string ToEdit(int productId)
        => GetUriByAction(nameof(AcceptProductController.Edit), new { productId });
}
