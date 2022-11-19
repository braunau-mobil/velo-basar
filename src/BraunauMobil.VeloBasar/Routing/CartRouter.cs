using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class CartRouter
    : ControllerRouter
    , ICartRouter
{
    public CartRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<CartController>(), linkGenerator)
    { }

    public string ToAdd()
        => GetUriByAction(nameof(CartController.Add));

    public string ToCheckout()
        => GetUriByAction(nameof(CartController.Checkout));

    public string ToClear()
        => GetUriByAction(nameof(CartController.Clear));

    public string ToDelete(int productId)
        => GetUriByAction(nameof(CartController.Delete), new { productId });

    public string ToIndex()
        => GetUriByAction(nameof(CartController.Index));
}
