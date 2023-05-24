using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class CreateNewAsync
    : IDisposable
{
    public CreateNewAsync()
    {
        //Connection = new SqliteConnection("DataSource=:memory:");
        Connection = new SqliteConnection("Data Source=c:\\temp\\test.db;");
        Connection.Open();

        DbContextOptions<VeloDbContext> options = new DbContextOptionsBuilder<VeloDbContext>()
            .UseSqlite(Connection)
            .Options;

        // Create the schema in the database
        using (var context = new VeloDbContext(Clock.Object, options))
        {
            context.Database.Migrate();
        }

        Db = new VeloDbContext(Clock.Object, options);

        Sut = new AcceptProductService(Db, Helpers.CreateActualLocalizer());
    }
    
    public AcceptProductService Sut { get; }

    protected SqliteConnection Connection { get; }

    public VeloDbContext Db { get; }

    public Mock<IClock> Clock { get; } = new();

    public virtual void Dispose()
    {
        Db.Dispose();
        Connection.Dispose();
        Clock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Test()
    {
        //  @todo SQLIte Migrate does not work
        await Task.CompletedTask;
        ////  Arrange
        //Db.AcceptSessions.Add(new AcceptSessionEntity()
        //{
        //    Id = 1
        //});
        //await Db.SaveChangesAsync();

        ////  Act
        //AcceptProductModel result = await Sut.CreateNewAsync(1);

        ////  Assert
    }
}
