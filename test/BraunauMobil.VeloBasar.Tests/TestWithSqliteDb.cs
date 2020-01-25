using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests
{
    public class TestWithSqliteDb : TestBase, IDisposable, INumberContext
    {
        public TestWithSqliteDb()
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
        }

        protected SqliteConnection Connection { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {
                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }
        }

        protected async Task RunOn(Func<VeloRepository, Task> runTest)
        {
            Contract.Requires(runTest != null);

            var options = new DbContextOptionsBuilder<VeloRepository>()
                    .UseSqlite(Connection)
                    .Options;

            // Create the schema in the database
            using (var context = new VeloRepository(options))
            {
                await context.Database.MigrateAsync();
            }

            // Use a clean instance of the context to run the test
            using (var context = new VeloRepository(options))
            {
                await runTest(context);
            }
        }

        public int NextNumber(Basar basar, TransactionType transactionType)
        {
            return 1;
        }

        public async Task CreateNewNumberAsync(Basar basar, TransactionType type)
        {
            await Task.Run(() => { });
        }
    }
}
