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
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.DataGenerator;

public class Program
{
    public static async Task Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        Random rand = new();

        StringBuilder sb = new();

        sb.AppendLine("Sellers");
        sb.AppendLine();

        string[] countries = ["Austria", "Germany"];

        foreach (var x in Enumerable.Range(0, 8))
        {
            string firstName = rand.GetRandomElement(Names.FirstNames);
            string lastName = rand.GetRandomElement(Names.FirstNames);
            sb.AppendLine($"{{ \"BankAccountHolder\", \"{firstName} {firstName[0]}.{lastName[0]}. {lastName}\" }},");
            sb.AppendLine($"{{ \"City\", \"{rand.GetRandomElement(Names.Cities)}\" }},");
            sb.AppendLine($"{{ \"CountryId\", ID.Countries.{rand.GetRandomElement(countries)} }},");
            sb.AppendLine($"{{ \"EMail\", \"{firstName.ToLower().Replace(" ", "")}@{lastName.ToLower().Replace(" ", "")}.me\" }},");
            sb.AppendLine($"{{ \"FirstName\", \"{firstName}\" }},");
            sb.AppendLine($"{{ \"HasNewsletterPermission\", {(rand.Next(0, 2) == 0).ToString().ToLower()} }},");
            sb.AppendLine("{ \"IBAN\", \"\" },");
            sb.AppendLine($"{{ \"LastName\", \"{lastName}\" }},");
            sb.AppendLine($"{{ \"Street\", \"{rand.GetRandomElement(Names.Streets)} {rand.Next(1, 50)}\" }},");
            sb.Append("{ \"PhoneNumber\", \"");
            for (int counter = 0; counter < rand.Next(8, 11); counter++)
            {
                sb.Append(rand.Next(0, 10));
            }
            sb.AppendLine("\" },");
            sb.AppendLine($"{{ \"ZIP\", \"{rand.Next(1, 10)}{rand.Next(1, 10)}{rand.Next(1, 10)}{rand.Next(1, 10)}\" }},");

            sb.AppendLine();
        }

        sb.AppendLine("--------------------------------------------------------------------------");
        sb.AppendLine("Products");
        sb.AppendLine();


        string[] typeNames = [
            "Unicycle",
            "RoadBike",
            "MansCityBike",
            "WomansCityBike",
            "ChildrensBike",
            "Scooter",
            "EBike",
            "SteelSteed",
            ];
        string[] colors = [
            "red",
            "blue",
            "green",
            "yellow",
            "orange",
            "purple",
            "pink",
            "brown",
            "gray",
            "turquoise",
            "lavender",
            "maroon",
            "indigo",
            "cyan",
            "olive",
            "peach",
            "teal",
            "magenta",
            "beige",
            "slate",
            ];

        foreach (var x in Enumerable.Range(0, 30))
        {
            string brand = rand.GetRandomElement(Names.BrandNames);
            sb.AppendLine($"{{ \"TypeId\", ID.ProductTypes.{rand.GetRandomElement(typeNames)} }},");
            sb.AppendLine($"{{ \"Brand\", \"{brand}\" }},");
            sb.AppendLine($"{{ \"Color\", \"{rand.GetRandomElement(colors)}\" }},");
            if (rand.Next(0, 2) == 0)
            {
                sb.AppendLine($"{{ \"FrameNumber\", \"{Guid.NewGuid().ToString()[..rand.Next(7, 10)]}\" }},");
            }
            sb.AppendLine($"{{ \"Description\", \"{brand[..].ToUpper()}_{rand.Next(10000, 999999)}\" }},");
            sb.AppendLine($"{{ \"TireSize\", \"{rand.GetRandomElement(Names.TireSizes)}\" }},");
            sb.AppendLine($"{{ \"Price\", {Math.Round((decimal)rand.GetGaussian(100, 50), 2)}M }},");
            sb.AppendLine();
        }

        string fileName = @"c:\temp\testData.txt";
        File.WriteAllText(fileName, sb.ToString());
        ProcessStartInfo info = new(fileName)
        {
            UseShellExecute = true
        };
        Process.Start(info);

        //await GeneratePostgresAsync("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=velobasar;Pooling=true;");
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
