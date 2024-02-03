using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Data;

public sealed class DatabaseMigrator(VeloDbContext db, ILogger<DatabaseMigrator> logger)
{
    public void Migrate()
    {
        logger.LogInformation("Migrate database");

        if (!db.IsInitialized())
        {
            logger.LogInformation("Database is not initialized, no need for migration");
            return;
        }

        db.Database.Migrate();
    }
}
