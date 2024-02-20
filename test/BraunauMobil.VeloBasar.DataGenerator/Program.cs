using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.DataGenerator.Mockups;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Globalization;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.DataGenerator;

public class Program
{
    public static async Task Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");


        await GeneratePostgresAsync("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=velobasar;Pooling=true;");
    }

    private static async Task GeneratePostgresAsync(string connectionString)
    {
        await GenerateAsync(options =>
        {
            options.UseNpgsql(connectionString);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        });
    }

    private static async Task GenerateAsync(Action<DbContextOptionsBuilder> dbOptionsAction)
    {
        ServiceCollection services = new();
        services
            .AddLogging(logging =>
            {
                logging
                    .AddDebug()
                    .AddConsole()
                ;
            })
            .AddDefaultIdentity<IdentityUser>()
            .AddEntityFrameworkStores<VeloDbContext>();            
        services
            .AddDbContext<VeloDbContext>(dbOptionsAction);

        services
            .AddBusinessLogic()
            .AddSingleton<DataGeneratorService>()
            .AddSingleton<IFormatProvider>(CultureInfo.DefaultThreadCurrentCulture!)
            .AddSingleton<IClock, ClockMock>()
            .AddSingleton<IStatusPushService, StatusPushServiceMock>()
            .AddSingleton(CreateActualLocalizer())
            .AddSingleton<ITransactionDocumentGenerator, TransactionDocumentServiceMock>()
            .AddSingleton<IProductLabelGenerator, ProductLabelServiceMock>()
            .AddSingleton<IColorProvider, ColorProviderMock>()
        ;

        services
            .Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        DataGeneratorService dataGenerator = serviceProvider.GetRequiredService<DataGeneratorService>();

        dataGenerator.Contextualize(new DataGeneratorConfiguration
        {
            BasarCount = 1,
            FirstBasarDate = new DateTime(2063, 04, 05),
            GenerateCountries = true,
            GenerateProductTypes = true,
            GenerateZipCodes = true,
            Seed = 666,
            AdminUserEMail = "nope@nope.abc",
            SimulateBasar = true
        });
        await dataGenerator.GenerateAsync();
    }

    public static IStringLocalizer<SharedResources> CreateActualLocalizer()
    {
        IOptions<LocalizationOptions> options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        ResourceManagerStringLocalizerFactory factory = new(options, NullLoggerFactory.Instance);
        return new StringLocalizer<SharedResources>(factory);
    }
}
