using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public sealed class SetupService
    : ISetupService
{
    private readonly VeloDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public SetupService(VeloDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task CreateDatabaseAsync()
    {
        if (_db.IsSQLITE())
        {
            await _db.Database.EnsureCreatedAsync();
        }
        else
        {
            await _db.Database.MigrateAsync();
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
        
        await _userManager.CreateAsync(adminUser, "root");

        if (config.GenerateCountries)
        {
            _db.Countries.Add(new CountryEntity
            {
                Iso3166Alpha3Code = "AUT",
                Name = "Österreich",
                State = ObjectState.Enabled
            });
            _db.Countries.Add(new CountryEntity
            {
                Iso3166Alpha3Code = "GER",
                Name = "Deutschland",
                State = ObjectState.Enabled
            });
            await _db.SaveChangesAsync();
        }

        if (config.GenerateProductTypes)
        {
            GenerateProductTypes();
        }

        if (config.GenerateZipCodes)
        {
            _db.ZipCodes.AddRange(new ZipCollection(await _db.Countries.ToListAsync()));
        }

        await _db.SaveChangesAsync();
    }

    private void GenerateProductTypes()
    {
        foreach (string productTypeName in Names.ProductTypeNames)
        {
            _db.ProductTypes.Add(new ProductTypeEntity
            {
                Name = productTypeName,
                State = ObjectState.Enabled
            });
        }
    }
}
