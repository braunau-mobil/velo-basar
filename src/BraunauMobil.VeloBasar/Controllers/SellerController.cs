using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Crud;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class SellerController
    : AbstractCrudController<SellerEntity, SellerListParameter>
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
        SellerEntity seller = await _sellerService.CreateNewAsync();
        if (id.HasValue)
        {
            seller = await _sellerService.GetAsync(id.Value);
        }

        SellerCreateForAcceptanceModel model = new(seller);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateForAcceptance(SellerEntity seller)
    {
        ArgumentNullException.ThrowIfNull(seller);

        SetValidationResult(_sellerValidator.Validate(seller));

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

    public async Task<IActionResult> Details(int activeBasarId, int id)
    {
        SellerDetailsModel model = await _sellerService.GetDetailsAsync(activeBasarId, id);
        return View(model);
    }

    public async Task<IActionResult> Labels(int activeBasarId, int id)
    {
        FileDataEntity fileData = await _sellerService.GetLabelsAsync(activeBasarId, id);
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
                return Redirect(_router.Product.ToDetails(id));
            }
        }

        IPaginatedList<SellerEntity> items = await _sellerService.GetManyAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.SearchString, parameter.State, parameter.ValueState);
        ListModel<SellerEntity, SellerListParameter> model = new(items, parameter);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SearchForAcceptance(SellerSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        SetValidationResult(_searchValidator.Validate(searchModel));

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

    public async Task<IActionResult> Settle(int activeBasarId, int id)
    {
        int settlementId = await _sellerService.SettleAsync(activeBasarId, id);
        return Redirect(_router.Transaction.ToSucess(settlementId));
    }
}
