using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
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
            _services.AddLocalization(options =>
                options.ResourcesPath = "Resources"
            );
            Startup.RegisterServices(_services);
        }

        protected IBasarContext BasarContext { get; private set; }
        protected IBrandContext BrandContext { get; private set; }
        protected ICountryContext CountryContext { get; private set; }
        protected IDataGeneratorContext DataGeneratorContext { get; private set; }
        protected IFileStoreContext FileStoreContext { get; private set; }
        protected INumberContext NumberContext { get; private set; }
        protected IProductContext ProductContext { get; private set; }
        protected IProductTypeContext ProductTypeContext { get; private set; }
        protected ISellerContext SellerContext { get; private set; }
        protected ISettingsContext SettingsContext { get; private set; }
        protected ISetupContext SetupContext { get; private set; }
        protected IStatisticContext StatisticContext { get; private set; }
        protected ITransactionContext TransactionContext { get; private set; }
        protected IPrintService PrintService { get; private set; }
        protected IZipMapContext ZipMapContext { get; private set; }

        protected async Task RunOnInitializedDb(Func<Task> action)
        {
            await RunOnInitializedDb(async db => await action());
        }
        protected async Task RunOnInitializedDb(Func<VeloRepository, Task> action)
        {
            Contract.Requires(action != null);
            
            using var serviceProvider = _services.BuildServiceProvider();
            BasarContext = serviceProvider.GetRequiredService<IBasarContext>();
            BrandContext = serviceProvider.GetRequiredService<IBrandContext>();
            CountryContext = serviceProvider.GetRequiredService<ICountryContext>();
            DataGeneratorContext = serviceProvider.GetRequiredService<IDataGeneratorContext>();
            FileStoreContext = serviceProvider.GetRequiredService<IFileStoreContext>();
            NumberContext = serviceProvider.GetRequiredService<INumberContext>();
            ProductContext = serviceProvider.GetRequiredService<IProductContext>();
            ProductTypeContext = serviceProvider.GetRequiredService<IProductTypeContext>();
            SellerContext = serviceProvider.GetRequiredService<ISellerContext>();
            SettingsContext = serviceProvider.GetRequiredService<ISettingsContext>();
            SetupContext = serviceProvider.GetRequiredService<ISetupContext>();
            StatisticContext = serviceProvider.GetRequiredService<IStatisticContext>();
            TransactionContext = serviceProvider.GetRequiredService<ITransactionContext>();
            PrintService = serviceProvider.GetRequiredService<IPrintService>();
            ZipMapContext = serviceProvider.GetRequiredService<IZipMapContext>();

            // Create the schema in the database
            var options = new DbContextOptionsBuilder<VeloRepository>()
                    .UseSqlite(Connection)
                    .Options;

            // Create the schema in the database
            using var context = new VeloRepository(options);
            await SetupContext.InitializeDatabaseAsync(new InitializationConfiguration());

            await action(context);
        }
    }
}
