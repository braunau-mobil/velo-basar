using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ProductsViewModel : ListViewModel<Product>
    {
        public ProductsViewModel(Basar basar, IEnumerable<Product> list) : base(basar, list)
        {
        }

        public string GetStatusClass(ProductStatus status)
        {
            return $"ProductStatus ProductStatus-{status.ToString()}";
        }

        public bool CanCancel(Product product)
        {
            return product.Status == ProductStatus.Sold;
        }

        public IDictionary<string, string> GetItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("productId", product.Id.ToString());
            return route;
        }
    }
}
