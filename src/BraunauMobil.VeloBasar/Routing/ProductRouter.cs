using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class ProductRouter
    : ControllerRouter
    , IProductRouter
{
    public ProductRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<ProductController>(), linkGenerator)
    { }

    public string ToDetails(int id)
        => GetUriByAction(nameof(ProductController.Details), new { id });

    public string ToEdit(int id)
        => GetUriByAction(nameof(ProductController.Edit), new { id });

    public string ToLabel(int id)
        => GetUriByAction(nameof(ProductController.Label), new { id });

    public string ToList()
        => ToList(new ProductListParameter());

    public string ToList(int pageIndex)
        => ToList(new ProductListParameter { PageIndex = pageIndex });

    public string ToList(int? pageSize, int pageIndex)
        => ToList(new ProductListParameter { PageIndex = pageIndex, PageSize = pageSize });

    public string ToList(ListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        return GetUriByAction(nameof(ProductController.List), parameter);
    }

    public string ToList(ProductListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        return GetUriByAction(nameof(ProductController.List), parameter);
    }

    public string ToLock(int id)
        => GetUriByAction(nameof(ProductController.Lock), new { id });

    public string ToSetAsLost(int id)
        => GetUriByAction(nameof(ProductController.Lost), new { id });

    public string ToUnlock(int id)
        => GetUriByAction(nameof(ProductController.UnLock), new { id });
}
