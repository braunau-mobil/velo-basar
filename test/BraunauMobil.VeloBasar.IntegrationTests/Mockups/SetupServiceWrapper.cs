using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class SetupServiceWrapper(VeloDbContext db, UserManager<IdentityUser> userManager, IStringLocalizer<SharedResources> localizer)
    : ISetupService
{
    private static bool _createdOnce = false;
    private readonly SetupService _actual = new(db, userManager, localizer);

    public async Task InitializeDatabaseAsync(InitializationConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(db);

        await _actual.InitializeDatabaseAsync(config);
    }

    public async Task MigrateDatabaseAsync()
    {
        _createdOnce.Should().BeFalse();

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();

        _createdOnce = true;
    }
}
