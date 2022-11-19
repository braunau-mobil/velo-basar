using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class BasarController
    : AbstractVeloController
{
    private readonly IBasarService _service;
    private readonly IVeloRouter _router;

    public BasarController(IBasarService service, IVeloRouter router)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _router = router ?? throw new ArgumentNullException(nameof(router));
    }

    public IActionResult ActiveBasarDetails(int activeBasarId)
        => Redirect(_router.Basar.ToDetails(activeBasarId));

    public async Task<IActionResult> Details(int id)
    {
        BasarDetailsModel model = await _service.GetDetailsAsync(id);
        return View(model);
    }
}
