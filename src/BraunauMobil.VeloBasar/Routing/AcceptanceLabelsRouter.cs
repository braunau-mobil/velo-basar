using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class AcceptanceLabelsRouter
    : ControllerRouter
    , IAcceptanceLabelsRouter
{
    public AcceptanceLabelsRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<AcceptanceLabelsController>(), linkGenerator)
    { }

    public string ToDownload(int id)
        => GetUriByAction(nameof(AcceptanceLabelsController.Download), new { id });

    public string ToSelect()
        => GetUriByAction(nameof(AcceptanceLabelsController.Select));        
}
