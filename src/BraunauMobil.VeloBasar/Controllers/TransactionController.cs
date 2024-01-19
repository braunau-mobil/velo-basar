using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class TransactionController
    : AbstractVeloController
{
    private readonly ITransactionService _transactionService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IVeloRouter _router;
    private readonly IValidator<TransactionSuccessModel> _transactionSuccessValidator;

    public TransactionController(ITransactionService transactionService, IVeloRouter router, SignInManager<IdentityUser> signInManager, IValidator<TransactionSuccessModel> transactionSuccessValidator)
    {
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _transactionSuccessValidator = transactionSuccessValidator ?? throw new ArgumentNullException(nameof(transactionSuccessValidator));
    }

    public async Task<IActionResult> Cancel(int id)
    {
        TransactionEntity transaction = await _transactionService.GetAsync(id);
        return Redirect(_router.Cancel.ToSelectProducts(transaction.Id));
    }

    public async Task<IActionResult> Details(int id)
    {
        TransactionEntity transaction = await _transactionService.GetAsync(id);     
        return View(transaction);
    }

    public async Task<IActionResult> Success(int id)
    {
        TransactionEntity transaction = await _transactionService.GetAsync(id);
        TransactionSuccessModel model = new(transaction)
        {
            OpenDocument = true
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Success(int id, decimal amountGiven)
    {
        TransactionEntity transactionEntity = await _transactionService.GetAsync(id, amountGiven);
        TransactionSuccessModel model = new(transactionEntity)
        {
            AmountGiven = amountGiven
        };

        SetValidationResult(await _transactionSuccessValidator.ValidateAsync(model));

        return View(model);
    }

    public async Task<IActionResult> Document(int id)
    {
        FileDataEntity fileData = await _transactionService.GetDocumentAsync(id);
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public async Task<IActionResult> List(TransactionListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        if (parameter.TransactionType != TransactionType.Sale && !_signInManager.IsSignedIn(HttpContext.User))
        {
            return Redirect(_router.ToLogin());
        }

        IPaginatedList<TransactionEntity> items = await _transactionService.GetManyAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.BasarId, parameter.TransactionType, parameter.SearchString);
        ListModel<TransactionEntity, TransactionListParameter> model = new (items, parameter);
        return View(model);
    }
}
