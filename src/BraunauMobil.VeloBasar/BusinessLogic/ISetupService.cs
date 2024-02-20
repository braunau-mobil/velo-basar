namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ISetupService
{
    Task InitializeDatabaseAsync(InitializationConfiguration config);

    Task MigrateDatabaseAsync();
}
