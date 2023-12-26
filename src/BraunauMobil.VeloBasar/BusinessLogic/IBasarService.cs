using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IBasarService
    : ICrudService<BasarEntity, ListParameter>
{
    Task<int?> GetActiveBasarIdAsync();

    Task<string> GetBasarNameAsync(int id);

    Task<BasarDetailsModel> GetDetailsAsync(int id);    
}
