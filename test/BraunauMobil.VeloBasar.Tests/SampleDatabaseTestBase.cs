using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Crud;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests;

public class SampleDatabaseTestBase
    : IDisposable
{
    public SampleDatabaseTestBase()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-AT"); ;

        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        ServiceCollection services = new();
        services
          .AddDefaultIdentity<IdentityUser>()
          .AddEntityFrameworkStores<VeloDbContext>();
        services
          .AddDbContext<VeloDbContext>(options =>
          {
              options.UseSqlite(Connection);
          });

        services
            .AddBusinessLogic()
            .AddScoped<BasarCrudService>()
            .AddSingleton(Clock.Object)
            .AddSingleton(StatusPushService.Object)
            .AddSingleton<INumberService, MemoryNumberService>()
            .AddSingleton(Helpers.CreateActualLocalizer())
            .AddSingleton<ITransactionDocumentService, TransactionDocumentServiceMock>()
            .AddSingleton<IProductLabelService, ProductLabelServiceMock>()
            .AddSingleton<IColorProvider, OnlyBlackColorProvider>()
        ;        

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ServiceScope = serviceProvider.CreateScope();
        
        IDataGeneratorService dataGenerator = ServiceScope.ServiceProvider.GetRequiredService<IDataGeneratorService>();

        dataGenerator.Contextualize(new DataGeneratorConfiguration
        {
            BasarCount = 1,
            FirstBasarDate = new DateTime(2063, 04, 05),
            GenerateBrands = true,
            GenerateCountries = true,
            GenerateProductTypes = true,
            GenerateZipCodes = true,
            Seed = 666,
            AdminUserEMail = "nope@nope.abc",
            SimulateBasar = true
        });
        dataGenerator.GenerateAsync().Wait();
    }

    public virtual void Dispose()
    {
        ServiceScope.Dispose();
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public Mock<IClock> Clock { get; } = new();

    protected SqliteConnection Connection { get; }

    protected IServiceScope ServiceScope { get; }

    protected Mock<IStatusPushService> StatusPushService { get; } = new();
}
