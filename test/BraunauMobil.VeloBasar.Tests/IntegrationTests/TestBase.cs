using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xan.AspNetCore.Mvc.Filters;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public class TestBase
    : IDisposable
{
    private readonly SqliteConnection _connection;

    public TestBase()
    {
        ServiceCollection services = new();

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
            .AddDefaultIdentity<IdentityUser>()
            .AddEntityFrameworkStores<VeloDbContext>();

        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        services
            .AddDbContext<VeloDbContext>(options =>
            {
                options.UseSqlite(_connection)
                    .EnableSensitiveDataLogging();
            })
            .AddScoped<LinkGenerator, MockLinkGenerator>()
            .AddVeloRendering()
            .AddVeloRouting()
            .AddBusinessLogic()
            .AddSingleton<DatabaseMigrator>()
            .AddSingleton<IClock>(Clock)
            .AddValidatorsFromAssemblyContaining<SellerSearchModelValidator>()
            ;

        services.AddVeloCrud();
        
        services
            .ConfigureVeloCookies()
            .ConfigureVeloIdentity()
        ;

        Services = services
            .BuildServiceProvider();

        using IServiceScope scope = Services.CreateScope();
        DatabaseMigrator db = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        db.Migrate();
    }

    protected MockClock Clock { get; } = new();
    
    protected IServiceProvider Services { get; }

    public virtual void Dispose()
    {
        _connection.Dispose(); 
        GC.SuppressFinalize(this);
    }

    protected void Do<TController>(Action<TController> what)
        where TController : Controller
    {
        Services.Do(what);
    }

    protected async Task Do<TController>(Func<TController, Task> what)
        where TController : Controller
    {
        await Services.Do(what);
    }

    protected TResult Do<TController, TResult>(Func<TController, TResult> what)
        where TController : Controller
    {
        return Services.Do(what);
    }

    protected async Task<TResult> Do<TController, TResult>(Func<TController, Task<TResult>> what)
        where TController : Controller
    {
        return await Services.Do(what);
    }
}
