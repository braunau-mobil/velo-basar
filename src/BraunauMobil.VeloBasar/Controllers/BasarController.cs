using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Controllers;

[Authorize]
public sealed class BasarController
    : AbstractCrudController<BasarEntity, ListParameter, IBasarRouter, IBasarService>
{
    public BasarController(IBasarService service, IBasarRouter router, ICrudModelFactory<BasarEntity, ListParameter> modelFactory, IValidator<BasarEntity> validator)
        : base(service, router, modelFactory, validator)
    {
    }

    [AllowAnonymous]
    public IActionResult ActiveBasarDetails(int basarId)
        => Redirect(Router.ToDetails(basarId));

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        BasarDetailsModel model = await Service.GetDetailsAsync(id);
        return View(model);
    }

    public override async Task<IActionResult> List(ListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        if (int.TryParse(parameter.SearchString, out int basarId))
        {
            return await Task.FromResult(Redirect(Router.ToDetails(basarId)));
        }

        return await base.List(parameter);
    }
}
