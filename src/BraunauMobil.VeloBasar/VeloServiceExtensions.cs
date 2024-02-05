using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Parameters;
using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar;

public static class VeloServiceExtensions
{
    public static IServiceCollection AddVeloCrud(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .AddScoped<ICrudModelFactory<BasarEntity, ListParameter>, BasarCrudModelFactory>()
            .AddScoped<ICrudModelFactory<CountryEntity, ListParameter>, CountryCrudModelFactory>()
            .AddScoped<ICrudModelFactory<ProductTypeEntity, ListParameter>, ProductTypeCrudModelFactory>()
            .AddScoped<ICrudModelFactory<SellerEntity, SellerListParameter>, SellerCrudModelFactory>()
            ;
    }

    public static IServiceCollection AddVeloOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

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

        return services;
    }

    public static IServiceCollection ConfigureVeloCookies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
            .Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
            .ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/security/login";
                options.LogoutPath = "/security/logout";
            });
    }

    public static IServiceCollection ConfigureVeloIdentity(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services
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
    }
}
