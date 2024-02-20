using BraunauMobil.VeloBasar.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class AppContextWrapper(IWebHostEnvironment env, VeloDbContext db)
    : IAppContext
{
    private readonly VeloBasarAppContext _actual = new(env, db);

    public string Version => _actual.Version;

    public bool DevToolsEnabled()
        => _actual.DevToolsEnabled();

    public async Task<bool> NeedsInitialSetupAsync()
    {
        try
        {
            await db.Users.AnyAsync();
        }
        catch (SqliteException)
        {
            //  If an exception is thrown, the users table does not exist, therefore the database needs to be initialized
            return true;
        }
        return false;
    }

    public async Task<bool> NeedsMigrationAsync()
        => await Task.FromResult(false);
}
