using BraunauMobil.VeloBasar.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests;

public class SqliteTestBase
    : IDisposable
{
    public SqliteTestBase()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        DbContextOptions<VeloDbContext> options = new DbContextOptionsBuilder<VeloDbContext>()
            .UseSqlite(Connection)
            .Options;

        using (VeloDbContext context = new (Clock.Object, options))
        {
            context.Database.EnsureCreated();
        }

        Db = new (Clock.Object, options);
    }
    
    protected SqliteConnection Connection { get; }

    public VeloDbContext Db { get; }

    public Mock<IClock> Clock { get; } = new();

    public virtual void Dispose()
    {
        Db.Dispose();
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
