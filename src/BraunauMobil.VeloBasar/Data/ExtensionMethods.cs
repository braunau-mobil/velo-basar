using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ExtensionMethods
    {
        public static IQueryable<Brand> DefaultOrder(this IQueryable<Brand> brands)
        {
            return brands.OrderBy(b => b.Name);
        }
        public static IQueryable<Product> DefaultOrder(this IQueryable<Product> products)
        {
            return products.OrderBy(p => p.Id);
        }
        public static IQueryable<ProductType> DefaultOrder(this IQueryable<ProductType> productTypes)
        {
            return productTypes.OrderBy(pt => pt.Name);
        }
        public static IQueryable<Seller> DefaultOrder(this IQueryable<Seller> sellers)
        {
            return sellers.OrderBy(s => s.Id);
        }

        public static async Task<bool> ExistsAsync(this IQueryable<Brand> brands, int id)
        {
            return await brands.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this IQueryable<Product> products, int id)
        {
            return await products.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this IQueryable<ProductType> productTypes, int id)
        {
            return await productTypes.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this IQueryable<ProductsTransaction> transactions, int id)
        {
            return await transactions.AnyAsync(p => p.Id == id);
        }
        public static async Task<bool> ExistsAsync(this IQueryable<Seller> sellers, int id)
        {
            return await sellers.AnyAsync(s => s.Id == id);
        }

        public static async Task<Basar> GetAsync(this DbSet<Basar> basars, int id)
        {
            return await basars.FindAsync(id);
        }
        public static async Task<Brand> GetAsync(this DbSet<Brand> brands, int id)
        {
            return await brands.FindAsync(id);
        }
        public static async Task<FileStore> GetAsync(this DbSet<FileStore> fileStores, int id)
        {
            return await fileStores.FindAsync(id);
        }
        public static async Task<Product> GetAsync(this IQueryable<Product> products, int id)
        {
            return await products.IncludeAll().FirstOrDefaultAsync(p => p.Id == id);
        }
        public static async Task<ProductsTransaction> GetAsync(this IQueryable<ProductsTransaction> transactions, int id)
        {
            return await transactions.IncludeAll().FirstOrDefaultAsync(t => t.Id == id);
        }
        public static async Task<ProductsTransaction> GetAsync(this IQueryable<ProductsTransaction> transactions, Basar basar, TransactionType type, int number)
        {
            return await transactions.IncludeAll().FirstOrDefaultAsync(t => t.BasarId == basar.Id && t.Type == type && t.Number == number);
        }
        public static async Task<ProductType> GetAsync(this DbSet<ProductType> productTypes, int id)
        {
            return await productTypes.FindAsync(id);
        }
        public static async Task<Seller> GetAsync(this IQueryable<Seller> sellers, int id)
        {
            return await sellers.IncludeAll().FirstOrDefaultAsync(s => s.Id == id);
        }

        public static IQueryable<Brand> GetMany(this IQueryable<Brand> brands, string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return brands.DefaultOrder();
            }
            return brands.Where(Expressions.BrandSearch(searchString)).DefaultOrder();
        }
        public static IQueryable<Product> GetMany(this IQueryable<Product> products, string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return products.IncludeAll().DefaultOrder();
            }

            return products.Where(Expressions.ProductSearch(searchString)).IncludeAll().DefaultOrder();
        }
        public static IQueryable<Product> GetMany(this IQueryable<Product> products, IList<int> ids)
        {
            return products.Where(p => ids.Contains(p.Id)).IncludeAll().DefaultOrder();
        }
        public static IQueryable<ProductsTransaction> GetMany(this IQueryable<ProductsTransaction> transactions, TransactionType transactionType, Basar basar, string searchString)
        {
            return  transactions.GetMany(transactionType, basar, Expressions.TransactionSearch(searchString));
        }
        public static IQueryable<ProductsTransaction> GetMany(this IQueryable<ProductsTransaction> transactions, TransactionType transactionType, Basar basar, int sellerId)
        {
            return transactions.GetMany(transactionType, basar, t => t.SellerId == sellerId);
        }
        public static IQueryable<ProductsTransaction> GetMany(this IQueryable<ProductsTransaction> transactions, TransactionType transactionType, Basar basar, Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            var result = transactions.Where(t => t.Type == transactionType && t.Basar.Id == basar.Id);

            if (additionalPredicate != null)
            {
                return result.Where(additionalPredicate).IncludeAll();
            }

            return result.IncludeAll();
        }
        public static IQueryable<ProductType> GetMany(this IQueryable<ProductType> productTypes, string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return productTypes.DefaultOrder();
            }
            return productTypes.Where(Expressions.ProductTypeSearch(searchString)).DefaultOrder();
        }
        public static IQueryable<Seller> GetMany(this IQueryable<Seller> sellers, string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return sellers.IncludeAll().DefaultOrder();
            }

            return sellers.Where(Expressions.SellerSearch(searchString)).IncludeAll().DefaultOrder();
        }
        public static IQueryable<Seller> GetMany(this IQueryable<Seller> sellers, string firstName, string lastName)
        {
            return sellers.Where(Expressions.SellerSearch(firstName, lastName)).IncludeAll().DefaultOrder();
        }

        public static IQueryable<Product> IncludeAll(this IQueryable<Product> products)
        {
            return products
                .Include(p => p.Brand)
                .Include(p => p.Type);
        }
        public static IQueryable<ProductsTransaction> IncludeAll(this IQueryable<ProductsTransaction> transactions)
        {
            return transactions
                .Include(t => t.Basar)
                .Include(t => t.Seller)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Brand)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Type);
        }
        public static IQueryable<Seller> IncludeAll(this IQueryable<Seller> sellers)
        {
            return sellers
                .Include(s => s.Country);
        }
        
        public static IEnumerable<Product> WhereStorageState(this IEnumerable<Product> products, StorageStatus state)
        {
            return products.Where(p => p.StorageStatus == state);
        }
        public static IEnumerable<Product> WhereStorageStateIsNot(this IEnumerable<Product> products, StorageStatus state)
        {
            return products.Where(p => p.StorageStatus != state);
        }
    }
}
