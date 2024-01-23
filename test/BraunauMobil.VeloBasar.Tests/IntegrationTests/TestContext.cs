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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Xan.AspNetCore;
using Xan.AspNetCore.Http;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Filters;
using Xan.Extensions;
using Xan.Extensions.Tasks;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public sealed class TestContext
{
    private readonly CultureInfo _initialCultureInfo = CultureInfo.CurrentCulture;
    private const string _connectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;
    private readonly WebApplication _app;

    public TestContext()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Logging.AddConsole();

        _connection = new SqliteConnection(_connectionString);
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

            VeloDbContext db = scope.ServiceProvider.GetRequiredService<VeloDbContext>();
            db.Database.EnsureCreated();
        }

        _app = app;

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
    }

    private void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        services
           .AddDefaultIdentity<IdentityUser>()
           .AddEntityFrameworkStores<VeloDbContext>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<PageSizeFilter>();
            options.Filters.Add<BasarIdFilter>();
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
            .AddScoped<LinkGenerator, LinkGeneratorMock>()
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
            .AddHostedService<QueuedHostedService>()
            .AddSingleton(Cookies);

        services.AddVeloOptions(configuration);
        services.AddVeloCrud();

        PageSizeCookie.Options.MaxAge = TimeSpan.FromDays(2);

        services
            .ConfigureVeloCookies()
            .ConfigureVeloIdentity()
        ;
    }

    public ClockMock Clock { get; } = new();

    public CookiesMock Cookies { get; } = new ();

    public IServiceProvider Services { get => _app.Services; }

    public void Dispose()
    {
        CultureInfo.CurrentUICulture = _initialCultureInfo;
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
