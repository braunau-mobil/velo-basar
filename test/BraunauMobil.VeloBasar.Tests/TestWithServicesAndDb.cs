using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests
{
    public class TestWithServicesAndDb : TestWithSqliteDb
    {
        private readonly ServiceCollection _services = new ServiceCollection();

        public TestWithServicesAndDb()
        {
            _services.AddDbContext<VeloRepository>(options =>
            {
                options.UseSqlite(Connection);
            });
            _services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<VeloRepository>();
        }

        protected void AddLogic()
        {
            Startup.RegisterServices(_services);
        }
        protected void AddLocalization()
        {
            _services.AddLocalization(options =>
                options.ResourcesPath = "Resources"
            );
        }
        protected async Task RunWithServiesAndDb(Func<ServiceProvider, Task> runTest)
        {
            Contract.Requires(runTest != null);

            // Create the schema in the database
            var options = new DbContextOptionsBuilder<VeloRepository>()
                    .UseSqlite(Connection)
                    .Options;

            // Create the schema in the database
            using (var context = new VeloRepository(options))
            {
                context.Database.EnsureCreated();
            }

            using (var serviceProvicer = _services.BuildServiceProvider())
            {
                await runTest(serviceProvicer);
            }
        }
    }
}
