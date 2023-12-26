using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public class BasarRouter
    : CrudRouter<BasarEntity>
    , IBasarRouter
{
    public BasarRouter(LinkGenerator linkGenerator)
        : base(linkGenerator)
    { }

    public string ToActiveBasarDetails()
        => GetUriByAction(nameof(BasarController.ActiveBasarDetails));

    public string ToDetails(int id)
        => GetUriByAction(nameof(BasarController.Details), new { id });
}
