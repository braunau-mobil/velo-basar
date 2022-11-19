using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AcceptanceLabelsController
    : AbstractVeloController
{
    private readonly ITransactionService _transactionService;
    private readonly VeloTexts _txt;

    public AcceptanceLabelsController(ITransactionService transactionService, VeloTexts txt)
    {
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));
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

        TransactionEntity? acceptance = await _transactionService.FindAsync(model.ActiveBasarId, TransactionType.Acceptance, model.Number);
        if (acceptance == null)
        {
            ModelState.AddModelError(nameof(AcceptanceLabelsModel.Number), _txt.NoAcceptanceFound(model.Number));
            return View(new AcceptanceLabelsModel { Number = model.Number });
        }

        model.OpenDocument = true;
        model.Id = acceptance.Id;
        return View(model);
    }
}
