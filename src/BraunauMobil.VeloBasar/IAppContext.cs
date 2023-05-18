namespace BraunauMobil.VeloBasar;

public interface IAppContext
{
    string Version { get; }

    bool DevToolsEnabled();

    bool IsDatabaseInitialized();
}
