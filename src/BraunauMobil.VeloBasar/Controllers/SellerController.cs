using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Crud;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class SellerController
    : AbstractCrudController<SellerEntity, SellerListParameter, ISellerRouter, ISellerService>
{
    private readonly ISellerService _sellerService;
    private readonly IVeloRouter _router;
    private readonly IValidator<SellerSearchModel> _searchValidator;
    private readonly IValidator<SellerEntity> _sellerValidator;

    public SellerController(ISellerService sellerService, IValidator<SellerSearchModel> searchValidator, IVeloRouter router, ISellerRouter sellerRouter, SellerCrudModelFactory modelFactory, IValidator<SellerEntity> sellerValidator)
        : base(sellerService, sellerRouter, modelFactory, sellerValidator)
    {
        _sellerService = sellerService ?? throw new ArgumentNullException(nameof(sellerService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _searchValidator = searchValidator ?? throw new ArgumentNullException(nameof(searchValidator));
        _sellerValidator = sellerValidator ?? throw new ArgumentNullException(nameof(sellerValidator));
    }

    public async Task<IActionResult> CreateForAcceptance(int? id)
    {
        SellerEntity seller;
        if (id.HasValue)
        {
            seller = await _sellerService.GetAsync(id.Value);
        }
        else
        {
            seller = await _sellerService.CreateNewAsync();
        }

        SellerCreateForAcceptanceModel model = new(seller);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateForAcceptance(SellerEntity seller)
    {
        ArgumentNullException.ThrowIfNull(seller);

        SetValidationResult(await _sellerValidator.ValidateAsync(seller));

        if (ModelState.IsValid)
        {
            if (seller.Id != 0)
            {
                await _sellerService.UpdateAsync(seller);
            }
            else
            {
                await _sellerService.CreateAsync(seller);
            }
            return Redirect(_router.AcceptSession.ToStartForSeller(seller.Id));
        }

        SellerCreateForAcceptanceModel model = new(seller);
        return View(model);
    }

    public async Task<IActionResult> Details(SellerDetailsParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        SellerDetailsModel model = await _sellerService.GetDetailsAsync(parameter.BasarId, parameter.Id);
        return View(model);
    }

    public async Task<IActionResult> Labels(int basarId, int id)
    {
        FileDataEntity fileData = await _sellerService.GetLabelsAsync(basarId, id);
        return File(fileData.Data, fileData.ContentType, fileData.FileName);
    }

    public override async Task<IActionResult> List(SellerListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        if (int.TryParse(parameter.SearchString, out int id))
        {
            if (await _sellerService.ExistsAsync(id))
            {
                return Redirect(_router.Seller.ToDetails(id));
            }
        }

        return await base.List(parameter);
    }

    [HttpPost]
    public async Task<IActionResult> SearchForAcceptance(SellerSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        SetValidationResult(await _searchValidator.ValidateAsync(searchModel));

        IReadOnlyList<SellerEntity> foundSellers;
        if (ModelState.IsValid)
        {
            foundSellers = await _sellerService.GetManyAsync(searchModel.FirstName, searchModel.LastName);
        }
        else
        {
            foundSellers = Array.Empty<SellerEntity>();
        }

        SellerEntity seller = new()
        { 
            FirstName = searchModel.FirstName,
            LastName = searchModel.LastName
        };
        SellerCreateForAcceptanceModel model = new(seller, true, foundSellers);
        return View(nameof(CreateForAcceptance), model);
    }

    public async Task<IActionResult> Settle(int basarId, int id)
    {
        int settlementId = await _sellerService.SettleAsync(basarId, id);
        return Redirect(_router.Transaction.ToSucess(settlementId));
    }

    public async Task<IActionResult> TriggerStatusPush(int basarId, int id)
    {
        await _sellerService.TriggerStatusPushAsync(basarId, id);

        return RedirectToReferer();
    }

    protected override IActionResult RedirectToOrigin(SellerEntity entity, string? origin)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (origin == "details")
        {
            return Redirect(_router.Seller.ToDetails(entity.Id));
        }
        return base.RedirectToOrigin(entity, origin);
    }
}
