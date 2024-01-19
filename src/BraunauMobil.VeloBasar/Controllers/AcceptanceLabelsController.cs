using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AcceptanceLabelsController
    : AbstractVeloController
{
    private readonly ITransactionService _transactionService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public AcceptanceLabelsController(ITransactionService transactionService, IStringLocalizer<SharedResources> localizer)
    {
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public async Task<IActionResult> Download(int id)
    {
        FileDataEntity fileData = await _transactionService.GetAcceptanceLabelsAsync(id);
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public IActionResult Select()
        => View(new AcceptanceLabelsModel());

    [HttpPost]
    public async Task<IActionResult> Select(AcceptanceLabelsModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TransactionEntity? acceptance = await _transactionService.FindAsync(model.BasarId, TransactionType.Acceptance, model.Number);
        if (acceptance == null)
        {
            ModelState.AddModelError(nameof(AcceptanceLabelsModel.Number), _localizer[VeloTexts.NoAcceptanceFound, model.Number]);
            return View(new AcceptanceLabelsModel { Number = model.Number });
        }

        model.OpenDocument = true;
        model.Id = acceptance.Id;
        return View(model);
    }
}
