using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ExtensionMethods
    {
        public static async Task<bool> ExistsAsync(this DbSet<Seller> dbSet, int id)
        {
            return await dbSet.AnyAsync(s => s.Id == id);
        }

        public static IEnumerable<Product> Sold(this IEnumerable<Product> products)
        {
            return products.Where(p => p.Status == ProductStatus.Sold);
        }

        public static IEnumerable<Product> NotSold(this IEnumerable<Product> products)
        {
            return products.Where(p => p.Status != ProductStatus.Sold);
        }
    }
}
