using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.IntegrationTests.Mockups;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<VeloDbContext>));
            services.Remove(dbContextDescriptor);

            ServiceDescriptor? dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            if (dbConnectionDescriptor is not null)
            {
                services.Remove(dbConnectionDescriptor);
            }

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                SqliteConnection connection = new("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<VeloDbContext>((container, options) =>
            {
                DbConnection connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

            ServiceDescriptor clockDescriptor = services.Single(d => d.ServiceType == typeof(IClock));
            services.Remove(clockDescriptor);
            services.AddSingleton<IClock>(X.Clock);

            ServiceDescriptor appContextDescriptor = services.Single(d => d.ServiceType == typeof(IAppContext));
            services.Remove(appContextDescriptor);
            services.AddScoped<IAppContext, AppContextWrapper>();

            ServiceDescriptor setupServiceDescriptor = services.Single(d => d.ServiceType == typeof(ISetupService));
            services.Remove(setupServiceDescriptor);
            services.AddScoped<ISetupService, SetupServiceWrapper>();

            ServiceDescriptor transactionDocumentGeneratorDescriptor = services.Single(d => d.ServiceType == typeof(ITransactionDocumentGenerator));
            services.Remove(transactionDocumentGeneratorDescriptor);
            services.AddScoped<ITransactionDocumentGenerator, JsonTransactionDocumentGenerator>();

            ServiceDescriptor productLabelGeneratorDescriptor = services.Single(d => d.ServiceType == typeof(IProductLabelGenerator));
            services.Remove(productLabelGeneratorDescriptor);
            services.AddScoped<IProductLabelGenerator, JsonProductLabelGenerator>();
        });

        builder.UseEnvironment("IntegrationTests");
    }
}
