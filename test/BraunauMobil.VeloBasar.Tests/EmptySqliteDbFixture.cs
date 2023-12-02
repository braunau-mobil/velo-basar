using BraunauMobil.VeloBasar.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests;

public sealed class EmptySqliteDbFixture
    : IDbFixture
{
    private readonly SqliteConnection _connection;

    public EmptySqliteDbFixture()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        DbContextOptions<VeloDbContext> options = new DbContextOptionsBuilder<VeloDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        using (VeloDbContext context = new(Clock.Object, options))
        {
            context.Database.EnsureCreated();
        }

        Db = new(Clock.Object, options);
    }    

    public VeloDbContext Db { get; }

    public Mock<IClock> Clock { get; } = new();

    public void Dispose()
    {
        Db.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
