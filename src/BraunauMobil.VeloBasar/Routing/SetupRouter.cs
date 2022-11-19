using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class SetupRouter
    : ControllerRouter
    , ISetupRouter
{
    public SetupRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<SetupController>(), linkGenerator)
    { }

    public string ToInitialSetup()
        => GetUriByAction(nameof(SetupController.InitialSetup));
}
