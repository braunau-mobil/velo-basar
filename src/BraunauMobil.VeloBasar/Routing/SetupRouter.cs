using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class SetupRouter(LinkGenerator linkGenerator)
    : ControllerRouter(MvcHelper.ControllerName<SetupController>(), linkGenerator)
    , ISetupRouter
{
    public string ToInitialSetup()
        => GetUriByAction(nameof(SetupController.InitialSetup));

    public string ToMigrateDatabase()
        => GetUriByAction(nameof(SetupController.MigrateDatabase));
}
