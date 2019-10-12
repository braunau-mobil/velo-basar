using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.Models
{
    public static class ExtensionMethods
    {
        public static bool CanSettle(this IEnumerable<Product> products)
        {
            return products.Any(p => p.CanSettle());
        }
    }
}
