using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class DevRouter
    : ControllerRouter
    , IDevRouter
{
    public DevRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<DevController>(), linkGenerator)
    { }

    public string ToDangerZone()
        => GetUriByAction(nameof(DevController.DangerZone));

    public string ToDropDatabase()
        => GetUriByAction(nameof(DevController.DropDatabase));

    public string ToUnlockAllUsers()
        => GetUriByAction(nameof(DevController.UnlockAllUsers));
}
