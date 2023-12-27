using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Tests.Mockups;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BraunauMobil.VeloBasar.Tests;

public sealed class SampleSqliteDbFixture
    : IDbFixture
{
    private const string _defaultDatabaseFileName = "VeloBasarTest.db";

    private readonly string _databaseFileName;

    public SampleSqliteDbFixture()
    {
        _databaseFileName = Guid.NewGuid().ToString();
        File.Copy(_defaultDatabaseFileName, _databaseFileName);

        DbContextOptions<VeloDbContext> options = new DbContextOptionsBuilder<VeloDbContext>()
            .UseSqlite($"DataSource={_databaseFileName}")
            .EnableSensitiveDataLogging()
            .Options;

        Db = new(Clock, options);
    }

    public VeloDbContext Db { get; }

    public ClockMock Clock { get; } = new();

    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }
}
