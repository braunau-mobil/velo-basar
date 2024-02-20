namespace BraunauMobil.VeloBasar;

public interface IAppContext
{
    string Version { get; }

    bool DevToolsEnabled();

    Task<bool> NeedsInitialSetupAsync();

    Task<bool> NeedsMigrationAsync();
}
