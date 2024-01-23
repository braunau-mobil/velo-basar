using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                options.Filters.Add<IsDatabaseInitializedFilter>();
                options.Filters.Add<PageSizeFilter>();
                options.Filters.Add<BasarIdFilter>();
                options.Filters.Add<ActiveSessionIdFilter>();
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
            .AddHttpClient()
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

        services.AddVeloOptions(configuration);
        services.AddVeloCrud();

        PageSizeCookie.Options.MaxAge = TimeSpan.FromDays(2);

        services
            .ConfigureVeloCookies()
            .ConfigureVeloIdentity()
        ;
    }
}
