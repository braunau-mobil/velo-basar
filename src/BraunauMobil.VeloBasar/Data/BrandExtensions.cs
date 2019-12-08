using BraunauMobil.VeloBasar.Models;
using System.Linq;

namespace BraunauMobil.VeloBasar.Data
{
    public static class BrandExtensions
    {
        public static IQueryable<Brand> DefaultOrder(this IQueryable<Brand> brands)
        {
            return brands.OrderBy(b => b.Name);
        }
    }
}
