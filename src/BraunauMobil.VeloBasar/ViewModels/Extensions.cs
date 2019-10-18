using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public static class Extensions
    {
        public static ListViewModel<Product> CreateListViewModel(this IEnumerable<ProductToTransaction> items, Basar basar)
        {
            return new ListViewModel<Product>(basar, items.Select(pt => pt.Product).ToList());
        }
    }
}
