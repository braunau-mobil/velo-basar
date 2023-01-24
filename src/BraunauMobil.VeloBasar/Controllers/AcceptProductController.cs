using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AcceptProductController
    : AbstractVeloController
{
    private readonly IAcceptProductService _acceptProductService;    
    private readonly IValidator<ProductEntity> _validator;
    private readonly IVeloRouter _router;

    public AcceptProductController(IAcceptProductService acceptProductService, IVeloRouter router, IValidator<ProductEntity> validator)
    {
        _acceptProductService = acceptProductService ?? throw new ArgumentNullException(nameof(acceptProductService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IActionResult> Create(int sessionId)
    {
        AcceptProductModel model = await _acceptProductService.CreateNewAsync(sessionId);
        return CreateEdit(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        SetValidationResult(_validator.Validate(product));

        if (ModelState.IsValid)
        {
            await _acceptProductService.CreateAsync(product);

            return Redirect(_router.AcceptProduct.ToCreate(product.SessionId));
        }
        
        AcceptProductModel model = await _acceptProductService.GetAsync(product.SessionId, product);
        return CreateEdit(model);
    }

    public async Task<IActionResult> Delete(int sessionId, int productId)
    {
        await _acceptProductService.DeleteAsync(productId);

        return Redirect(_router.AcceptProduct.ToCreate(sessionId));
    }

    public async Task<IActionResult> Edit(int productId)
    {
        AcceptProductModel model = await _acceptProductService.GetAsync(productId);
        return CreateEdit(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        SetValidationResult(_validator.Validate(product));

        if (ModelState.IsValid)
        {
            await _acceptProductService.UpdateAsync(product);

            return Redirect(_router.AcceptProduct.ToCreate(product.SessionId));
        }

        AcceptProductModel model = await _acceptProductService.GetAsync(product.SessionId, product);
        return CreateEdit(model);
    }

    private IActionResult CreateEdit(AcceptProductModel model)
        => View(nameof(CreateEdit), model);
}
