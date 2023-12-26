using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IProductTypeService
    : ICrudService<ProductTypeEntity, ListParameter>
{ }
