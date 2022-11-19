namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ISetupService
{
    Task CreateDatabaseAsync();
    
    Task InitializeDatabaseAsync(InitializationConfiguration config);
}
