using BraunauMobil.VeloBasar.Models;
using System.Linq;

namespace BraunauMobil.VeloBasar.Data
{
    public static class CountryExtensions
    {
        public static IQueryable<Country> DefaultOrder(this IQueryable<Country> countries)
        {
            return countries.OrderBy(c => c.Name);
        }
    }
}
