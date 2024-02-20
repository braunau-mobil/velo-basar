using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Controllers;

[Authorize]
public class CountryController
    : AbstractCrudController<CountryEntity, ListParameter, ICountryRouter, ICountryService>
{
    public CountryController(ICountryService service, ICountryRouter router, ICrudModelFactory<CountryEntity, ListParameter> modelFactory, IValidator<CountryEntity> validator)
        : base(service, router, modelFactory, validator)
    { }
}
