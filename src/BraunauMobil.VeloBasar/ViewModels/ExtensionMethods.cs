using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public static class ExtensionMethods
    {
        public static ListViewModel<Product> CreateListViewModel(this IEnumerable<ProductToTransaction> items, Basar basar)
        {
            return new ListViewModel<Product>(basar, items.Select(pt => pt.Product).ToList());
        }
    }
}
