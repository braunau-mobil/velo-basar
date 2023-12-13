﻿using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Crud;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Pdf;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
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

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public class TestBase
    : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly WebApplication _app;

    public TestBase()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Logging.AddConsole();

        _connection = new SqliteConnection("DataSource=:memory:");
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

        _app = app;
    }

    private void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        services
           .AddDefaultIdentity<IdentityUser>()
           .AddEntityFrameworkStores<VeloDbContext>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<PageSizeFilter>();
            options.Filters.Add<ActiveBasarEntityFilter>();
        })
            .AddApplicationPart(typeof(HomeController).Assembly)
            .AddViewLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

        services
            .AddDbContext<VeloDbContext>(options =>
            {
                _connection.Open();
                options.UseSqlite(_connection)
                    .EnableSensitiveDataLogging();
            })
            .AddHttpContextAccessor()
            .AddHttpClient()
            .AddScoped<LinkGenerator, MockLinkGenerator>()
            .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
            .AddSingleton<IClock>(Clock)
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

    protected MockClock Clock { get; } = new();

    protected IServiceProvider Services { get => _app.Services; }

    public virtual void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
