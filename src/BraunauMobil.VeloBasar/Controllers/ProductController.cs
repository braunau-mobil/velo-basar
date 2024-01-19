using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class ProductController
    : AbstractVeloController
{
    private readonly IProductService _productService;
    private readonly IVeloRouter _router;
    private readonly IValidator<ProductAnnotateModel> _productAnnotateValidator;
    private readonly IValidator<ProductEntity> _productValidator;

    public ProductController(IVeloRouter router, IProductService productService, IValidator<ProductAnnotateModel> productAnnotateValidator, IValidator<ProductEntity> productValidator)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _productAnnotateValidator = productAnnotateValidator ?? throw new ArgumentNullException(nameof(productAnnotateValidator));
        _productValidator = productValidator ?? throw new ArgumentNullException(nameof(productValidator));
    }

    public async Task<IActionResult> Details(int id)
    {
        ProductDetailsModel model = await _productService.GetDetailsAsync(id);
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ProductEntity product = await _productService.GetAsync(id);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        SetValidationResult(await _productValidator.ValidateAsync(product));

        if (ModelState.IsValid)
        {
            await _productService.UpdateAsync(product);
            return Redirect(_router.Product.ToDetails(product.Id));
        }

        return View(product);
    }

    public async Task<IActionResult> List(ProductListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        if (int.TryParse(parameter.SearchString, out int id))
        {
            if (await _productService.ExistsForBasarAsync(parameter.BasarId, id))
            {
                return Redirect(_router.Product.ToDetails(id));
            }
        }

        IPaginatedList<ProductEntity> items = await _productService.GetManyAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.BasarId, parameter.SearchString, parameter.StorageState, parameter.ValueState, parameter.Brand, parameter.ProductTypeId);
        ListModel<ProductEntity, ProductListParameter> model = new(items, parameter);
        return View(model);
    }

    public IActionResult Lock(int id)
    {
        ProductAnnotateModel model = new() { ProductId = id };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Lock(ProductAnnotateModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        SetValidationResult(await _productAnnotateValidator.ValidateAsync(model));

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _productService.LockAsync(model.ProductId, model.Notes);
        return Redirect(_router.Product.ToDetails(model.ProductId));
    }

    public IActionResult Lost(int id)
    {
        ProductAnnotateModel model = new() { ProductId = id };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Lost(ProductAnnotateModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        SetValidationResult(await _productAnnotateValidator.ValidateAsync(model));

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _productService.SetLostAsync(model.ProductId, model.Notes);
        return Redirect(_router.Product.ToDetails(model.ProductId));
    }

    public async Task<IActionResult> Label(int id)
    {
        FileDataEntity label = await _productService.GetLabelAsync(id);
        return File(label.Data, label.ContentType, label.FileName);
    }

    public IActionResult UnLock(int id)
    {
        ProductAnnotateModel model = new() { ProductId = id };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UnLock(ProductAnnotateModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        SetValidationResult(await _productAnnotateValidator.ValidateAsync(model));

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _productService.UnlockAsync(model.ProductId, model.Notes);
        return Redirect(_router.Product.ToDetails(model.ProductId));
    }
}
