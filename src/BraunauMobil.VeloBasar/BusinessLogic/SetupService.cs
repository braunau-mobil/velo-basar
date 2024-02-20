using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SetupService(VeloDbContext db, UserManager<IdentityUser> userManager, IStringLocalizer<SharedResources> localizer)
    : ISetupService
{
    public async Task CreateDatabaseAsync()
    {
        if (db.IsSQLITE())
        {
            await db.Database.EnsureCreatedAsync();
        }
        else
        {
            await db.Database.MigrateAsync();
        }
    }

    public async Task InitializeDatabaseAsync(InitializationConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        IdentityUser adminUser = new()
        {
            Email = config.AdminUserEMail,
            UserName = config.AdminUserEMail
        };
        
        await userManager.CreateAsync(adminUser, "root");

        if (config.GenerateCountries)
        {
            db.Countries.Add(new CountryEntity
            {
                Iso3166Alpha3Code = "AUT",
                Name = "Österreich",
                State = ObjectState.Enabled
            });
            db.Countries.Add(new CountryEntity
            {
                Iso3166Alpha3Code = "GER",
                Name = "Deutschland",
                State = ObjectState.Enabled
            });
            await db.SaveChangesAsync();
        }

        if (config.GenerateProductTypes)
        {
            GenerateProductTypes();
        }

        if (config.GenerateZipCodes)
        {
            db.ZipCodes.AddRange(new ZipCollection(await db.Countries.ToListAsync()));
        }

        await db.SaveChangesAsync();
    }

    private void GenerateProductTypes()
    {
        string[] textIds =
        [
            VeloTexts.DefaultProductTypeUnicycle,
            VeloTexts.DefaultProductTypeRoadBike,
            VeloTexts.DefaultProductTypeMensCityBike,
            VeloTexts.DefaultProductTypeWomansCityBike,
            VeloTexts.DefaultProductTypeChildrensBike,
            VeloTexts.DefaultProductTypeScooter,
            VeloTexts.DefaultProductTypeEBike,
            VeloTexts.DefaultProductTypeSteelSteed,
        ];

        foreach (string textId in textIds)
        {
            db.ProductTypes.Add(new ProductTypeEntity
            {
                Name = localizer[textId],
                State = ObjectState.Enabled
            });
        }
    }
}
