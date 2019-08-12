using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class ListModel : BasarPageModel
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<Seller> Sellers { get;set; }

        public async Task OnGetAsync(int? basarId, string currentFilter, string searchString, int? pageIndex)
        {
            await LoadBasarAsync(basarId);

            CurrentFilter = searchString;
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Seller> sellerIq = from s in Context.Seller
                                            select s;

            if (int.TryParse(searchString, out int id))
            {
                sellerIq = sellerIq.Where(s => s.Id == id);
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                sellerIq = sellerIq.Where(s => s.FirstName.Contains(searchString, System.StringComparison.InvariantCultureIgnoreCase) ||s.LastName.Contains(searchString, System.StringComparison.InvariantCultureIgnoreCase));
            }

            var pageSize = 20;
            Sellers = await PaginatedList<Seller>.CreateAsync(
                sellerIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public IDictionary<string, string> GetItemRoute(Seller seller)
        {
            var route = GetRoute();
            route.Add("sellerId", seller.Id.ToString());
            return route;
        }
    }
}
