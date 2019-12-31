using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<bool> CanDeleteAsync(Country item) =>  !await _db.Sellers.AnyAsync(s => s.CountryId == item.Id);
        public async Task<Country> CreateAsync(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }
        public async Task DeleteAsync(int id)
        {
            var country = await GetAsync(id);
            if (country != null)
            {
                _db.Countries.Remove(country);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id) => await _db.Countries.ExistsAsync(id);
        public async Task<Country> GetAsync(int id) => await _db.Countries.FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<Country> GetMany(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _db.Countries.DefaultOrder();
            }
            return _db.Countries.Where(CountrySearch(searchString)).DefaultOrder();
        }
        public SelectList GetSelectList() => new SelectList(_db.Countries, "Id", "Name");
        public async Task UpdateAsync(Country country)
        {
            _db.Attach(country).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private Expression<Func<Country, bool>> CountrySearch(string searchString)
        {
            if (int.TryParse(searchString, out int id))
            {
                return c => c.Id == id;
            }
            if (_db.IsPostgreSQL())
            {
                return c => EF.Functions.ILike(c.Name, $"%{searchString}%")
                || EF.Functions.ILike(c.Iso3166Alpha3Code, $"%{searchString}%");
            }
            return c => EF.Functions.Like(c.Name, $"%{searchString}%")
                || EF.Functions.Like(c.Iso3166Alpha3Code, $"%{searchString}%");
        }
    }
}
