using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Data
{
    public static class ExtensionMethods
    {
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
