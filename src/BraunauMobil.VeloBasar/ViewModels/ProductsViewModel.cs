using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ProductsViewModel : ListViewModel<Product>
    {
        public ProductsViewModel(Basar basar, IEnumerable<Product> list) : base(basar, list)
        {
        }

        public IDictionary<string, string> GetItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("productId", product.Id.ToString());
            return route;
        }
    }
}
