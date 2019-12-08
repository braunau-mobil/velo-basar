using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class SetupContext : ISetupContext
    {
        private readonly VeloRepository _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISettingsContext _settingsContext;
        private readonly ICountryContext _countryContext;

        public SetupContext(VeloRepository db, UserManager<IdentityUser> userManager, ISettingsContext settingsContext, ICountryContext countryContext)
        {
            _db = db;
            _userManager = userManager;
            _settingsContext = settingsContext;
            _countryContext = countryContext;
        }

        public async Task InitializeDatabaseAsync(InitializationConfiguration config)
        {
            Contract.Requires(config != null);

            await _db.Database.EnsureCreatedAsync();

            var adminUser = new IdentityUser
            {
                Email = config.AdminUserEMail,
                UserName = config.AdminUserEMail
            };
            await _userManager.CreateAsync(adminUser, "root");

            var settings = new VeloSettings
            {
                IsInitialized = true
            };
            await _settingsContext.UpdateAsync(settings);

            await _settingsContext.UpdateAsync(new PrintSettings());

            await _countryContext.CreateAsync(new Country
            {
                Iso3166Alpha3Code = "AUT",
                Name = "Österreich"
            });
            await _countryContext.CreateAsync(new Country
            {
                Iso3166Alpha3Code = "GER",
                Name = "Deutschland"
            });
            await _db.SaveChangesAsync();
        }
    }
}
