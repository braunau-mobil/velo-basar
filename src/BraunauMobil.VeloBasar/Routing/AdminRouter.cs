using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class AdminRouter
    : ControllerRouter
    , IAdminRouter
{
    public AdminRouter(LinkGenerator linkGenerator)
    : base(MvcHelper.ControllerName<AdminController>(), linkGenerator)
    { }

    public string ToCreateSampleAcceptanceDocument()
        => GetUriByAction(nameof(AdminController.CreateSampleAcceptanceDocument));

    public string ToCreateSampleLabels()
        => GetUriByAction(nameof(AdminController.CreateSampleLabels));

    public string ToCreateSampleSaleDocument()
        => GetUriByAction(nameof(AdminController.CreateSampleSaleDocument));

    public string ToCreateSampleSettlementDocument()
        => GetUriByAction(nameof(AdminController.CreateSampleSettlementDocument));

    public string ToPrintTest()
        => GetUriByAction(nameof(AdminController.PrintTest));
}
