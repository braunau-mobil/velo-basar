using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ExtensionMethods
    {
        public static async Task<bool> ExistsAsync(this DbSet<Brand> dbSet, int id)
        {
            return await dbSet.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this DbSet<Product> dbSet, int id)
        {
            return await dbSet.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this DbSet<ProductType> dbSet, int id)
        {
            return await dbSet.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this DbSet<ProductsTransaction> dbSet, int id)
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
        public static IQueryable<Product> IncludeAll(this DbSet<Product> products)
        {
            return products
                .Include(p => p.Brand)
                .Include(p => p.Type);
        }
        public static IQueryable<ProductsTransaction> IncludeAll(this DbSet<ProductsTransaction> transactions)
        {
            return transactions
                .Include(t => t.Basar)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Brand)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Type);
        }
        public static IQueryable<Product> Get(this DbSet<Product> products, IEnumerable<int> ids)
        {
            return products.IncludeAll().Where(p => ids.Contains(p.Id));
        }
        public static async Task<ProductsTransaction> GetAsync(this DbSet<ProductsTransaction> transactions, int id)
        {
            return await transactions.IncludeAll().FirstOrDefaultAsync(t => t.Id == id);
        }
        public static async Task<ProductsTransaction> GetAsync(this DbSet<ProductsTransaction> transactions, Basar basar, TransactionType type, int number)
        {
            return await transactions.IncludeAll().FirstOrDefaultAsync(t => t.BasarId == basar.Id && t.Type == type && t.Number == number);
        }
    }
}
