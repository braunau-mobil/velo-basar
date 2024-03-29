﻿using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

[Authorize]
public sealed class CancelController
    : AbstractVeloController
{
    private readonly ITransactionService _transactionService;
    private readonly IVeloRouter _router;
    private readonly IValidator<SelectSaleModel> _selectSaleValidator;
    private readonly IValidator<SelectProductsModel> _selectProductsValidator;

    public CancelController(IVeloRouter router, ITransactionService transactionService, IValidator<SelectSaleModel> selectSaleValidator, IValidator<SelectProductsModel> cancelValidator)
    {
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _selectSaleValidator = selectSaleValidator ?? throw new ArgumentNullException(nameof(selectSaleValidator));
        _selectProductsValidator = cancelValidator ?? throw new ArgumentNullException(nameof(cancelValidator));
    }

    public IActionResult SelectSale()
    {
        return View(new SelectSaleModel());
    }

    [HttpPost]
    public async Task<IActionResult> SelectSale(SelectSaleModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Sale = await _transactionService.FindAsync(model.BasarId, TransactionType.Sale, model.SaleNumber);

        SetValidationResult(await _selectSaleValidator.ValidateAsync(model));
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return Redirect(_router.Cancel.ToSelectProducts(model.Sale!.Id));
    }

    public async Task<IActionResult> SelectProducts(int id)
    {
        IReadOnlyList<ProductEntity> products = await _transactionService.GetProductsToCancelAsync(id);
        SelectProductsModel model = new()
        {
            TransactionId = id,
        };
        model.SetProducts(products);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SelectProducts(SelectProductsModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        SetValidationResult(await _selectProductsValidator.ValidateAsync(model));

        if (ModelState.IsValid)
        {
            IEnumerable<int> selectedProductIds = model.SelectedProductIds();
            int revertId = await _transactionService.CancelAsync(model.BasarId, model.TransactionId, selectedProductIds);
            return Redirect(_router.Transaction.ToSucess(revertId));
        }

        IReadOnlyList<ProductEntity> products = await _transactionService.GetProductsToCancelAsync(model.TransactionId);
        model.SetProducts(products);
        return View(model);
    }
}
