using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class ListModel : BasarPageModel, IPagination
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<Seller> Sellers { get;set; }

        public int PageIndex => Sellers.PageIndex;

        public int TotalPages => Sellers.TotalPages;

        public bool HasPreviousPage => Sellers.HasPreviousPage;

        public bool HasNextPage => Sellers.HasNextPage;

        string IPagination.Page => "/Sellers/List";

        public async Task<IActionResult> OnGetAsync(int basarId, string currentFilter, string searchString, int? pageIndex)
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

            if (int.TryParse(searchString, out int id))
            {
                if (await Context.Seller.ExistsAsync(id))
                {
                    return RedirectToPage("/Sellers/Details", new { basarId, sellerId = id });
                }
            }

            var sellerIq = Context.GetSellers(searchString);

            var pageSize = 20;
            Sellers = await PaginatedList<Seller>.CreateAsync(
                sellerIq.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }

        public IDictionary<string, string> GetItemRoute(Seller seller)
        {
            var route = GetRoute();
            route.Add("sellerId", seller.Id.ToString());
            return route;
        }

        public IDictionary<string, string> GetPaginationRoute()
        {
            return GetRoute();
        }
    }
}
