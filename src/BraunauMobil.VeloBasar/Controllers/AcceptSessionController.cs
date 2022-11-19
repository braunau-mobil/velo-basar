using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AcceptSessionController
    : AbstractVeloController
{
    private readonly IAcceptSessionService _acceptSessionService;
    private readonly IVeloRouter _router;
    private readonly VeloTexts _txt;

    public AcceptSessionController(IAcceptSessionService acceptSessionService, IVeloRouter router, VeloTexts txt)
    {
        _acceptSessionService = acceptSessionService ?? throw new ArgumentNullException(nameof(acceptSessionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
    }

    public async Task<IActionResult> Cancel(int sessionId, bool returnToList)
    {
        AcceptSessionEntity session = await _acceptSessionService.GetAsync(sessionId);
        if (session.IsCompleted)
        {
            return BadRequest(_txt.AcceptSessionIsCompleted(sessionId));
        }

        await _acceptSessionService.DeleteAsync(session.Id);
        Response.Cookies.ClearActiveAcceptSession();

        if (returnToList)
        {
            return Redirect(_router.AcceptSession.ToList());
        }
        return Redirect(_router.Seller.ToDetails(session.SellerId));
    }

    [Authorize]
    public async Task<IActionResult> List(AcceptSessionListParameter parameter, int activeBasarId)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        IPaginatedList<AcceptSessionEntity> items = await _acceptSessionService.GetAllAsync(parameter.PageSize.Value, parameter.PageIndex, activeBasarId, parameter.AcceptSessionState);
        ListModel<AcceptSessionEntity, AcceptSessionListParameter> model = new(items, parameter);
        return View(model);
    }

    public async Task<IActionResult> Start(int activeBasarId)
    {
        ArgumentNullException.ThrowIfNull(activeBasarId);

        IActionResult? result = await RedirectToActiveSessionAsync();
        if (result != null)
        {
            return result;
        }

        return Redirect(_router.Seller.ToCreateForAcceptance());
    }

    public async Task<IActionResult> StartForSeller(int sellerId, int activeBasarId)
    {
        ArgumentNullException.ThrowIfNull(activeBasarId);

        IActionResult? result = await RedirectToActiveSessionAsync();
        if (result != null)
        {
            return result;
        }

        AcceptSessionEntity acceptSession = await _acceptSessionService.CreateAsync(activeBasarId, sellerId);
        Response.Cookies.SetActiveAcceptSession(acceptSession);

        return Redirect(_router.AcceptProduct.ToCreate(acceptSession.Id));
    }

    public async Task<IActionResult> Submit(int sessionId)
    {
        int acceptanceId = await _acceptSessionService.SubmitAsync(sessionId);
        return Redirect(_router.Transaction.ToSucess(acceptanceId));
    }

    private async Task<IActionResult?> RedirectToActiveSessionAsync()
    {
        int? activeSessionId = Request.Cookies.GetActiveAcceptSessionId();
        if (activeSessionId.HasValue)
        {
            if (await _acceptSessionService.IsSessionRunning(activeSessionId))
            {
                return Redirect(_router.AcceptProduct.ToCreate(activeSessionId.Value));
            }
            Response.Cookies.ClearActiveAcceptSession();
        }

        return null;
    }
}
