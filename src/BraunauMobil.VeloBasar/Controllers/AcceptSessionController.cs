using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Extensions;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AcceptSessionController
    : AbstractVeloController
{
    private readonly IAcceptSessionService _acceptSessionService;
    private readonly IVeloRouter _router;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IActiveAcceptSessionCookie _cookie;

    public AcceptSessionController(IAcceptSessionService acceptSessionService, IVeloRouter router, IStringLocalizer<SharedResources> localizer, IActiveAcceptSessionCookie cookie)
    {
        _acceptSessionService = acceptSessionService ?? throw new ArgumentNullException(nameof(acceptSessionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _cookie = cookie ?? throw new ArgumentNullException(nameof(cookie));
    }

    public async Task<IActionResult> Cancel(int sessionId, bool returnToList)
    {
        AcceptSessionEntity session = await _acceptSessionService.GetAsync(sessionId);
        if (session.IsCompleted)
        {
            return BadRequest(_localizer[VeloTexts.AcceptSessionIsCompleted, sessionId]);
        }

        await _acceptSessionService.DeleteAsync(session.Id);
        _cookie.ClearActiveAcceptSession();

        if (returnToList)
        {
            return Redirect(_router.AcceptSession.ToList());
        }
        return Redirect(_router.Seller.ToDetails(session.SellerId));
    }

    [Authorize]
    public async Task<IActionResult> List(AcceptSessionListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        IPaginatedList<AcceptSessionEntity> items = await _acceptSessionService.GetAllAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.BasarId, parameter.AcceptSessionState);
        ListModel<AcceptSessionEntity, AcceptSessionListParameter> model = new(items, parameter);
        return View(model);
    }

    public IActionResult Start(int basarId)
    {
        ArgumentNullException.ThrowIfNull(basarId);

        IActionResult? result = RedirectToActiveSession();
        if (result != null)
        {
            return result;
        }

        return Redirect(_router.Seller.ToCreateForAcceptance());
    }

    public async Task<IActionResult> StartForSeller(int sellerId, int basarId)
    {
        ArgumentNullException.ThrowIfNull(basarId);

        IActionResult? result = RedirectToActiveSession();
        if (result != null)
        {
            return result;
        }

        AcceptSessionEntity acceptSession = await _acceptSessionService.CreateAsync(basarId, sellerId);
        _cookie.SetActiveAcceptSession(acceptSession);

        return Redirect(_router.AcceptProduct.ToCreate(acceptSession.Id));
    }

    public async Task<IActionResult> Submit(int sessionId)
    {
        int acceptanceId = await _acceptSessionService.SubmitAsync(sessionId);
        return Redirect(_router.Transaction.ToSucess(acceptanceId));
    }

    private IActionResult? RedirectToActiveSession()
    {
        int? activeSessionId = ViewData.GetActiveSessionId();
        if (activeSessionId.HasValue)
        {
            return Redirect(_router.AcceptProduct.ToCreate(activeSessionId.Value));
        }

        return null;
    }
}
