using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.Models
{
    public static class ExtensionMethods
    {
        public static IReadOnlyList<Product> GetProducts(this IEnumerable<ProductToTransaction> productToTransactions)
        {
            return productToTransactions.Select(pt => pt.Product).ToArray();
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

        public static decimal SumPrice(this IEnumerable<Product> products)
        {
            return products.Sum(p => p.Price);
        }
    }
}
