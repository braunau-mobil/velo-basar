using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class AcceptSessionRouter
    : ControllerRouter
    , IAcceptSessionRouter
{
    public AcceptSessionRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<AcceptSessionController>(), linkGenerator)
    { }

    public string ToCancel(int sessionId, bool returnToList = false)
    {
        var parameter = new
        {
            sessionId,
            returnToList
        };
        return GetUriByAction(nameof(AcceptSessionController.Cancel), parameter);
    }

    public string ToList()
        => ToList(new ListParameter());

    public string ToList(int pageIndex)
        => ToList(new ListParameter { PageIndex = pageIndex });

    public string ToList(int? pageSize, int pageIndex)
        => ToList(new ListParameter { PageIndex = pageIndex, PageSize = pageSize });

    public string ToList(ListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        return GetUriByAction(nameof(AcceptSessionController.List), parameter);
    }

    public string ToStart()
        => GetUriByAction(nameof(AcceptSessionController.Start));

    public string ToStartForSeller(int sellerId)
    {
        return GetUriByAction(nameof(AcceptSessionController.StartForSeller), new { sellerId });
    }

    public string ToSubmit(int sessionId)
        => GetUriByAction(nameof(AcceptSessionController.Submit), new { sessionId });
}
