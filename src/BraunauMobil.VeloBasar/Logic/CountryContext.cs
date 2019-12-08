using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class CountryContext : ICountryContext
    {
        private readonly VeloRepository _db;

        public CountryContext(VeloRepository db)
        {
            _db = db;
        }

        public async Task CreateAsync(Country country)
        {
            await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();
        }
        public SelectList GetSelectList() => new SelectList(_db.Countries, "Id", "Name");
    }
}
