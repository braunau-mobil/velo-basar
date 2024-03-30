using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class TransactionController(ITransactionService transactionService, IVeloRouter router, SignInManager<IdentityUser> signInManager, IValidator<TransactionSuccessModel> transactionSuccessValidator)
        : AbstractVeloController
{
    [Authorize]
    public async Task<IActionResult> Cancel(int id)
    {
        TransactionEntity transaction = await transactionService.GetAsync(id);
        return Redirect(router.Cancel.ToSelectProducts(transaction.Id));
    }

    public async Task<IActionResult> Details(int id)
    {
        TransactionEntity transaction = await transactionService.GetAsync(id);     
        return View(transaction);
    }

    public async Task<IActionResult> Success(int id)
    {
        TransactionEntity transaction = await transactionService.GetAsync(id);
        TransactionSuccessModel model = new(transaction, openDocument: true);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Success(int id, decimal amountGiven)
    {
        TransactionEntity transactionEntity = await transactionService.GetAsync(id, amountGiven);
        TransactionSuccessModel model = new(transactionEntity)
        {
            AmountGiven = amountGiven
        };

        SetValidationResult(await transactionSuccessValidator.ValidateAsync(model));

        return View(model);
    }

    public async Task<IActionResult> Document(int id)
    {
        FileDataEntity fileData = await transactionService.GetDocumentAsync(id);
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public async Task<IActionResult> List(TransactionListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        if (parameter.TransactionType != TransactionType.Sale && !signInManager.IsSignedIn(HttpContext.User))
        {
            return Redirect(router.ToLogin());
        }

        IPaginatedList<TransactionEntity> items = await transactionService.GetManyAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.BasarId, parameter.TransactionType, parameter.SearchString);
        ListModel<TransactionEntity, TransactionListParameter> model = new (items, parameter);
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Unsettle(int id)
    {
        int unsettlementId = await transactionService.UnsettleAsync(id);
        return Redirect(router.Transaction.ToSucess(unsettlementId));
    }
}
