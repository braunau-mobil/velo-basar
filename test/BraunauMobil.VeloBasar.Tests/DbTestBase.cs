using BraunauMobil.VeloBasar.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests;

public class DbTestBase
    : IDisposable
{
    private readonly SqliteConnection _connection;

    public DbTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        DbContextOptions<VeloDbContext> options = new DbContextOptionsBuilder<VeloDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        using (VeloDbContext context = new(Clock, options))
        {
            context.Database.EnsureCreated();
        }

        Db = new(Clock, options);
    }

    public virtual void Dispose()
    {
        Db.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public VeloDbContext Db { get; }

    public ClockMock Clock { get; } = new();
}
