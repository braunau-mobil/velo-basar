using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class SellerRouter
    : CrudRouter<SellerEntity>
    , ISellerRouter
{
    public SellerRouter(LinkGenerator linkGenerator)
        : base(linkGenerator)
    { }

    public string ToCreateForAcceptance()
        => GetUriByAction(nameof(SellerController.CreateForAcceptance));

    public string ToCreateForAcceptance(int id)
        => GetUriByAction(nameof(SellerController.CreateForAcceptance), new { id });

    public string ToSearchForAcceptance()
        => GetUriByAction(nameof(SellerController.SearchForAcceptance));

    public string ToDetails(int id)
        => GetUriByAction(nameof(SellerController.Details), new { id });

    public string ToList(SellerListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        return GetUriByAction(nameof(SellerController.List), parameter);
    }

    public string ToLabels(int id)
        => GetUriByAction(nameof(SellerController.Labels), new { id });

    public string ToSettle(int id)
        => GetUriByAction(nameof(SellerController.Settle), new { id });
}
