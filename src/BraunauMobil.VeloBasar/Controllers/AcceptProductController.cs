using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class AcceptProductController
    : AbstractVeloController
{
    private readonly IAcceptProductService _acceptProductService;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IQrImportService _qrImportService;
    private readonly IValidator<ProductEntity> _validator;
    private readonly IVeloRouter _router;

    public AcceptProductController(IAcceptProductService acceptProductService, IVeloRouter router, IValidator<ProductEntity> validator, IQrImportService qrImportService, IStringLocalizer<SharedResources> localizer)
    {
        _acceptProductService = acceptProductService ?? throw new ArgumentNullException(nameof(acceptProductService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _qrImportService = qrImportService ?? throw new ArgumentNullException(nameof(qrImportService));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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

        SetValidationResult(await _validator.ValidateAsync(product));

        if (ModelState.IsValid)
        {
            await _acceptProductService.CreateAsync(product);

            return Redirect(_router.AcceptProduct.ToCreate(product.SessionId));
        }
        
        AcceptProductModel model = await _acceptProductService.GetAsync(product.SessionId, product);
        return CreateEdit(model);
    }

    [HttpPost]
    public async Task<IActionResult> ScanQr(ProductEntity product, string? qrScanText)
    {
        ArgumentNullException.ThrowIfNull(product);

        ModelState.Clear();

        QrImportResult result = await _qrImportService.TryApplyToProductAsync(qrScanText, product);
        AddQrImportModelErrors(result);

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

        SetValidationResult(await _validator.ValidateAsync(product));

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

    private void AddQrImportModelErrors(QrImportResult result)
    {
        switch (result.Status)
        {
            case QrImportStatus.Empty:
                ModelState.AddModelError(string.Empty, _localizer[VeloTexts.QrScanEmpty]);
                break;
            case QrImportStatus.InvalidFormat:
                ModelState.AddModelError(string.Empty, _localizer[VeloTexts.QrScanInvalidData]);
                break;
            case QrImportStatus.UnsupportedSchemaVersion:
                ModelState.AddModelError(string.Empty, _localizer[VeloTexts.QrScanUnsupportedVersion]);
                break;
            case QrImportStatus.ChecksumMismatch:
                ModelState.AddModelError(string.Empty, _localizer[VeloTexts.QrScanChecksumMismatch]);
                break;
            case QrImportStatus.WrongKind:
                ModelState.AddModelError(string.Empty, _localizer[VeloTexts.QrScanWrongKindExpectedProduct, result.ActualKind ?? "?"]);
                break;
            case QrImportStatus.Success:
                if (result.UnmatchedProductType is not null)
                {
                    ModelState.AddModelError(nameof(ProductEntity.TypeId), _localizer[VeloTexts.QrScanProductTypeNotFound, result.UnmatchedProductType]);
                }
                break;
        }
    }
}
