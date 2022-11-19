namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IBasarService
{
    Task<int?> GetActiveBasarIdAsync();

    Task<string> GetBasarNameAsync(int id);

    Task<BasarDetailsModel> GetDetailsAsync(int id);
}
