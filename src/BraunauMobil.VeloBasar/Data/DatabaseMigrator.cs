using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Data;

public sealed class DatabaseMigrator
{
    private readonly VeloDbContext _db;
    private readonly ILogger<DatabaseMigrator> _logger;

    public DatabaseMigrator(VeloDbContext db, ILogger<DatabaseMigrator> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Migrate()
    {
        _logger.LogInformation("Migrate database");

        if (!_db.IsInitialized())
        {
            _logger.LogInformation("Database is not initialized, no need for migration");
            return;
        }

        _db.Database.Migrate();
    }
}
