using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Models
{
    public static class ExtensionMethods
    {
        public static IList<Product> GetProducts(this IEnumerable<ProductToTransaction> productToTransactions)
        {
            return productToTransactions.Select(pt => pt.Product).ToList();
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
            foreach (var product in products)
            {
                product.SetState(transactionType);
            }
        }
    }
}
