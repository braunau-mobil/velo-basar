using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
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
        public static IQueryable<Basar> DefaultOrder(this IQueryable<Basar> basars)
        {
            return basars.OrderBy(b => b.Date);
        }
        public static IQueryable<Brand> DefaultOrder(this IQueryable<Brand> brands)
        {
            return brands.OrderBy(b => b.Name);
        }
        public static IQueryable<Product> DefaultOrder(this IQueryable<Product> products) => products.OrderById();
        public static IQueryable<ProductType> DefaultOrder(this IQueryable<ProductType> productTypes) => productTypes.OrderById();
        public static IQueryable<Seller> DefaultOrder(this IQueryable<Seller> sellers) => sellers.OrderById();

        public static async Task<bool> ExistsAsync<T>(this IQueryable<T> models, int id) where T : IModel
        {
            return await models.AnyAsync(p => p.Id == id);
        }

        public static async Task<T> GetAsync<T>(this DbSet<T> models, int id) where  T : class, IModel
        {
            return await models.FindAsync(id);
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
        public static async Task<Seller> GetAsync(this IQueryable<Seller> sellers, int id)
        {
            return await sellers.IncludeAll().FirstOrDefaultAsync(s => s.Id == id);
        }

        public static IQueryable<Basar> GetEnabled(this IQueryable<Basar> basars)
        {
            return basars.Where(b => !b.IsLocked).DefaultOrder();
        }

        public static IQueryable<Basar> GetMany(this IQueryable<Basar> basars, string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return basars.DefaultOrder();
            }
            return basars.Where(Expressions.BasarSearch(searchString)).DefaultOrder();
        }
        public static IQueryable<Brand> GetMany(this IQueryable<Brand> brands, string searchString = null)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return brands.DefaultOrder();
            }
            return brands.Where(Expressions.BrandSearch(searchString)).DefaultOrder();
        }
        public static IQueryable<Product> GetMany(this IQueryable<Product> products, string searchString = null, StorageState? storageState = null, ValueStatus? valueState = null)
        {
            var result = products;
            if (!string.IsNullOrEmpty(searchString))
            {
                result = result.Where(Expressions.ProductSearch(searchString));
            }

            if (storageState != null)
            {
                result = result.WhereStorageState(storageState.Value);
            }

            if (valueState != null)
            {
                result = result.WhereValueState(valueState.Value);
            }

            return result.IncludeAll().DefaultOrder();
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

        public static async Task<Basar> GetNoTrackingAsync(this DbSet<Basar> basars, int id)
        {
            return await basars.AsNoTracking().FirstAsync(b => b.Id == id);
        }

        public static async Task<int?> GetUniqueEnabledAsync(this IQueryable<Basar> basars)
        {
            if (basars.Count(b => !b.IsLocked) == 1)
            {
                var basar = await basars.FirstAsync(b => !b.IsLocked);
                return basar.Id;
            }
            return null;
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

        public static IQueryable<T> OrderById<T>(this IQueryable<T> models) where T : IModel
        {
            return models.OrderBy(p => p.Id);
        }

        public static IQueryable<Product> WhereStorageState(this IQueryable<Product> products, StorageState state)
        {
            return products.Where(p => p.StorageState == state);
        }
        public static IEnumerable<Product> WhereStorageState(this IEnumerable<Product> products, StorageState state)
        {
            return products.Where(p => p.StorageState == state);
        }
        public static IEnumerable<Product> WhereStorageStateIsNot(this IEnumerable<Product> products, StorageState state)
        {
            return products.Where(p => p.StorageState != state);
        }

        public static IQueryable<Product> WhereValueState(this IQueryable<Product> products, ValueStatus valueState)
        {
            return products.Where(p => p.ValueStatus == valueState);
        }
    }
}
