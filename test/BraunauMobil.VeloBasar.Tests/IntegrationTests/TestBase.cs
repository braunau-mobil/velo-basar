using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Rendering;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
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
}
