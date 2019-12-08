using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests
{
    public class TestWithSqliteDb : IDisposable, INumberContext
    {
        private SqliteConnection _connection;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }

        [TestInitialize]
        public virtual void Init()
        { 
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }
        [TestCleanup]
        public virtual void Cleanup()
        {
            _connection?.Close();
        }
        protected async Task RunOn(Func<VeloRepository, Task> runTest)
        {
            Contract.Requires(runTest != null);

            var options = new DbContextOptionsBuilder<VeloRepository>()
                    .UseSqlite(_connection)
                    .Options;

            // Create the schema in the database
            using (var context = new VeloRepository(options))
            {
                context.Database.EnsureCreated();
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
            //  @todo
        }
    }
}
