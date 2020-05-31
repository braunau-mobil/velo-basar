using System;
using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.Models
{
    public static class ExtensionMethods
    {
        public static IReadOnlyList<Product> GetPayoutProducts(this IEnumerable<ProductToTransaction> productToTransactions)
        {
            return productToTransactions.Select(pt => pt.Product).Where(p => p.ShouldBePayedOut()).ToArray();
        }
        public static IReadOnlyList<Product> GetProducts(this IEnumerable<ProductToTransaction> productToTransactions)
        {
            return productToTransactions.Select(pt => pt.Product).ToArray();
        }
        public static IEnumerable<Product> GetProductsToPayout(this IEnumerable<Product> products)
        {
            return products.Where(p => p.StorageState == StorageState.Sold || p.StorageState == StorageState.Gone);
        }
        public static IEnumerable<Product> GetProductsToPickup(this IEnumerable<Product> products)
        {
            return products.Where(p => p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked);
        }
        public static IReadOnlyList<Product> GetSoldProducts(this IEnumerable<ProductToTransaction> productToTransactions)
        {
            return productToTransactions.Select(pt => pt.Product).Where(p => p.StorageState == StorageState.Sold).ToArray();
        }
        public static bool IsAllowed(this IEnumerable<Product> products, TransactionType transactionType)
        {
            return products.All(p => p.IsAllowed(transactionType));
        }
        public static bool IsAllowed(this IEnumerable<ProductToTransaction> products, TransactionType transactionType)
        {
            return products.GetProducts().All(p => p.IsAllowed(transactionType));
        }
        public static void SetState(this IEnumerable<Product> products, TransactionType transactionType)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            foreach (var product in products)
            {
                product.SetState(transactionType);
            }
        }

        public static decimal SumPrice(this IEnumerable<Product> products)
        {
            return products.Sum(p => p.Price);
        }
    }
}
