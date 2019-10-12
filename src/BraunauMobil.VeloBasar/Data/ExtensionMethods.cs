using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ExtensionMethods
    {
        public static async Task<bool> ExistsAsync(this DbSet<Product> dbSet, int id)
        {
            return await dbSet.AnyAsync(p => p.Id == id);
        }

        public static async Task<bool> ExistsAsync(this DbSet<Seller> dbSet, int id)
        {
            return await dbSet.AnyAsync(s => s.Id == id);
        }

        public static IEnumerable<Product> WhereStorageState(this IEnumerable<Product> products, StorageStatus state)
        {
            return products.Where(p => p.StorageStatus == state);
        }

        public static IQueryable<Product> WhereStorageState(this IQueryable<Product> products, StorageStatus state)
        {
            return products.Where(p => p.StorageStatus == state);
        }

        public static IEnumerable<Product> WhereStorageStateIsNot(this IEnumerable<Product> products, StorageStatus state)
        {
            return products.Where(p => p.StorageStatus != state);
        }
    }
}
