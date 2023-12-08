using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BraunauMobil.VeloBasar.Routing;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Logging;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Crud;
using BraunauMobil.VeloBasar.Controllers;
using Xan.AspNetCore.Mvc;
using System.Net.Http;
using Xan.Extensions;
using FluentValidation;
using Xan.Extensions.Tasks;
using Xan.AspNetCore;
using BraunauMobil.VeloBasar.Pdf;
using Xan.AspNetCore.Http;
using Xan.AspNetCore.Mvc.Filters;
using System.Globalization;
using BraunauMobil.VeloBasar.Cookies;

namespace BraunauMobil.VeloBasar;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddConsole();
        builder.Configuration.AddCommandLine(args);

        ApplicationSettings? applicationSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
        if (applicationSettings == null)
        {
            throw new InvalidOperationException($"No {nameof(ApplicationSettings)} section found.");
        }
        CultureInfo applicationCulture = CultureInfo.GetCultureInfo(applicationSettings.Culture);
        CultureInfo.DefaultThreadCurrentCulture = applicationCulture;
        CultureInfo.DefaultThreadCurrentUICulture = applicationCulture;

        ConfigureServices(builder.Services, builder.Configuration);

        WebApplication app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action}",
            defaults: new { controller = MvcHelper.ControllerName<HomeController>(), action = nameof(HomeController.Index) }
        );

        using (IServiceScope scope = app.Services.CreateScope())
        {
            ILogger<SharedResources> logger = scope.ServiceProvider.GetRequiredService<ILogger<SharedResources>>();
            VeloTexts.CheckIfAllIsTranslated(logger);
            
            DatabaseMigrator migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
            migrator.Migrate();
        }

        app.Run();
    }    

    private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        services
           .AddDefaultIdentity<IdentityUser>()
           .AddEntityFrameworkStores<VeloDbContext>();
        
        services.AddControllersWithViews(options =>
            {
                options.Filters.Add<PageSizeFilter>();
                options.Filters.Add<ActiveBasarEntityFilter>();
            })
            .AddViewLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

        services
            .AddDbContext<VeloDbContext>(options =>
            {
                ApplicationSettings? applicationSettings = configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
                if (applicationSettings == null)
                {
                    throw new InvalidOperationException($"No {nameof(ApplicationSettings)} section found.");
                }

                options.UseNpgsql(applicationSettings.ConnectionString);
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            })
            .AddHttpContextAccessor()
            .AddSingleton<HttpClient>(sp =>
            {
                SocketsHttpHandler socketsHandler = new()
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(2)
                };
                return new HttpClient(socketsHandler);
            })
            .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
            .AddSingleton<IClock, SystemClock>()
            .AddBusinessLogic()
            .AddVeloCookies()
            .AddVeloRendering()
            .AddVeloRouting()
            .AddPdf()
            .AddScoped<IAppContext, VeloBasarAppContext>()
            .AddScoped<DatabaseMigrator>()
            .AddScoped<SellerCrudModelFactory>()
            .AddValidatorsFromAssemblyContaining<SellerSearchModelValidator>()

            .AddHostedService<QueuedHostedService>();

        services
            .AddOptions<ApplicationSettings>()
                .Bind(configuration.GetSection(nameof(ApplicationSettings)))
                .ValidateDataAnnotations();        
        services
            .AddOptions<PrintSettings>()
                .Bind(configuration.GetSection(nameof(PrintSettings)))
                .ValidateDataAnnotations();
        services
            .AddOptions<WordPressStatusPushSettings>()
                .Bind(configuration.GetSection(nameof(WordPressStatusPushSettings)))
                .ValidateDataAnnotations();
        services
            .AddOptions<ExportSettings>()
                .Bind(configuration.GetSection(nameof(ExportSettings)))
                .ValidateDataAnnotations();

        services.AddCrud(options =>
        {
            options.AddController<BasarEntity, BasarCrudService, BasarCrudModelFactory>(true);
            options.AddController<CountryEntity, CountryCrudService, CountryCrudModelFactory>(true);
            options.AddController<ProductTypeEntity, ProductTypeCrudService, ProductTypeCrudModelFactory>(true);
        });

        PageSizeCookie.Options.MaxAge = TimeSpan.FromDays(2);

        services
            .Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
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
            })
            .ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/security/login";
                options.LogoutPath = "/security/logout";
            });
    }
}
