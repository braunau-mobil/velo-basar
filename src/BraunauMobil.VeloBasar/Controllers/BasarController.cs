using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class BasarController
    : AbstractCrudController<BasarEntity, ListParameter, IBasarRouter, IBasarService>
{
    public BasarController(IBasarService service, IBasarRouter router, ICrudModelFactory<BasarEntity, ListParameter> modelFactory, IValidator<BasarEntity> validator)
        : base(service, router, modelFactory, validator)
    {
    }

    public IActionResult ActiveBasarDetails(int activeBasarId)
        => Redirect(Router.ToDetails(activeBasarId));

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
