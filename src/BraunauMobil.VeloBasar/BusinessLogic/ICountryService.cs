using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ICountryService
    : ICrudService<CountryEntity, ListParameter>
{ }
