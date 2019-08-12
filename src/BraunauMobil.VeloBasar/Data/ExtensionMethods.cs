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

        public static IEnumerable<Product> State(this IEnumerable<Product> products, ProductStatus state)
        {
            return products.Where(p => p.Is(state));
        }

        public static IQueryable<Product> State(this IQueryable<Product> products, ProductStatus state)
        {
            return products.Where(p => p.Is(state));
        }

        public static IEnumerable<Product> NotState(this IEnumerable<Product> products, ProductStatus state)
        {
            return products.Where(p => p.IsNot(state));
        }

        public static bool Is(this Product product, ProductStatus state)
        {
            return product.Status == state;
        }

        public static bool IsNot(this Product product, ProductStatus state)
        {
            return product.Status != state;
        }
    }
}
