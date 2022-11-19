using BraunauMobil.VeloBasar.Controllers;

namespace BraunauMobil.VeloBasar.Routing;

public static class BasarRouterExtensions
{
    public static string ToActiveBasarDetails(this ICrudRouter<BasarEntity> router)
    {
        ArgumentNullException.ThrowIfNull(router);

        return router.GetUriByAction(nameof(BasarController.ActiveBasarDetails));
    }

    public static string ToDetails(this ICrudRouter<BasarEntity> router, int id)
    {
        ArgumentNullException.ThrowIfNull(router);
        
        return router.GetUriByAction(nameof(BasarController.Details), new { id });
    }
}
