using System.Globalization;
using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Pdf;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xan.AspNetCore;
using Xan.AspNetCore.Http;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Filters;
using Xan.Extensions;
using Xan.Extensions.Tasks;

namespace BraunauMobil.VeloBasar;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
public class Program
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddConsole();
        builder.Configuration.AddCommandLine(args);

        ApplicationSettings? applicationSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
        if (applicationSettings == null)
        {
            throw new InvalidOperationException($"No {nameof(ApplicationSettings)} section found.");
        }
        CultureInfo applicationCulture = GetCultureInfoInternal(applicationSettings.Culture);
        CultureInfo.DefaultThreadCurrentCulture = applicationCulture;
        CultureInfo.DefaultThreadCurrentUICulture = applicationCulture;

        ConfigureServices(builder.Services, builder.Configuration, applicationCulture);

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

            scope.ServiceProvider.EnsureDatabaseCreated();
        }

        await app.RunAsync();
    }    

    private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration, IFormatProvider formatProvider)
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
            .AddSingleton<IVeloCache, VeloCache>()
            .AddSingleton<IFormatProvider>(formatProvider)
            .AddBusinessLogic()
            .AddVeloCookies()
            .AddVeloRendering()
            .AddVeloRouting()
            .AddPdf()
            .AddScoped<IAppContext, VeloBasarAppContext>()
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

    public static Func<string, CultureInfo>? GetCultureInfo {get; set; }

    private static CultureInfo GetCultureInfoInternal(string name)
    {
        if (GetCultureInfo is not null)
        {
            return GetCultureInfo(name);
        }
        return CultureInfo.GetCultureInfo(name);
    }
}
