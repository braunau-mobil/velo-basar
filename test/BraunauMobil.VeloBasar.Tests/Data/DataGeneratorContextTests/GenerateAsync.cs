using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests.Data.DataGeneratorContextTests
{
    [TestClass]
    public class GenerateAsync : TestWithSqliteDb
    {
        [TestMethod]
        public async Task GenerateManyBasars()
        {
            // Create a new service provider to create a new in-memory database.
            var services = new ServiceCollection();
            services.AddDbContext<VeloRepository>(options =>
                 {
                     options.UseSqlite(_connection);
                 });
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<VeloRepository>();
            
            services.AddLocalization(options => 
                options.ResourcesPath = "Resources"
            );

            services.AddScoped<IBasarContext, BasarContext>();
            services.AddScoped<IBrandContext, BrandContext>();
            services.AddScoped<ICountryContext, CountryContext>();
            services.AddScoped<IDataGeneratorContext, DataGeneratorContext>();
            services.AddScoped<IFileStoreContext, FileStoreContext>();
            services.AddScoped<INumberContext, NumberContext>();
            services.AddScoped<IProductContext, ProductContext>();
            services.AddScoped<IProductTypeContext, ProductTypeContext>();
            services.AddScoped<ISellerContext, SellerContext>();
            services.AddScoped<ISettingsContext, SettingsContext>();
            services.AddScoped<ISetupContext, SetupContext>();
            services.AddScoped<IStatisticContext, StatisticContext>();
            services.AddScoped<ITransactionContext, TransactionContext>();

            services.AddScoped<IPrintService, PdfPrintService>();
            services.AddScoped<IVeloContext, DefaultVeloContext>();


            var provider = services.BuildServiceProvider();

            var dataGeneratorContext = provider.GetRequiredService<IDataGeneratorContext>();

            await dataGeneratorContext.GenerateAsync(new DataGeneratorConfiguration
            {
                AdminUserEMail = "dev@xaka.eu",
                BasarCount = 100,
                FirstBasarDate = new DateTime(2063, 4, 5),
                GenerateBrands = true,
                GenerateCountries = true,
                GenerateProductTypes = true,
                MaxAcceptancesPerSeller = 1,
                MaxSellers = 1,
                MeanPrice = 1,
                MeanProductsPerSeller = 1,
                MinAcceptancesPerSeller = 1,
                MinSellers = 1,
                StdDevPrice = 1,
                StdDevProductsPerSeller = 1
            });
        }
    }
}
