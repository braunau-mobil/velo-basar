using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public class ListViewModel<T> : BasarViewModel
    {
        public ListViewModel(Basar basar, IEnumerable<T> list) : base(basar)
        {
            List = list;
        }

        public IEnumerable<T> List
        {
            get;
            set;
        }

        public IDictionary<string, string> GetItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("productId", product.Id.ToString());
            return route;
        }
    }
}
