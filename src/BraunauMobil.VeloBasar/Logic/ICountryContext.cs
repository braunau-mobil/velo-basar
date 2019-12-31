using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ICountryContext
    {
        Task<Country> CreateAsync(Country country);
        Task<bool> CanDeleteAsync(Country item);
        Task DeleteAsync(int countryId);
        Task<bool> ExistsAsync(int countryId);
        Task<Country> GetAsync(int countryId);
        IQueryable<Country> GetMany(string searchString);
        SelectList GetSelectList();
        Task UpdateAsync(Country country);
    }
}
