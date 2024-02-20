using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Controllers;

[Authorize]
public class ProductTypeController
    : AbstractCrudController<ProductTypeEntity, ListParameter, IProductTypeRouter, IProductTypeService>
{
    public ProductTypeController(IProductTypeService service, IProductTypeRouter router, ICrudModelFactory<ProductTypeEntity, ListParameter> modelFactory, IValidator<ProductTypeEntity> validator)
        : base(service, router, modelFactory, validator)
    { }
}
