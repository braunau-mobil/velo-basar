using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class ListModel : BasarPageModel, ISearchable
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedListViewModel<Seller> Sellers { get;set; }

        public string MyPath => "/Sellers/List";

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
            Sellers = await PaginatedListViewModel<Seller>.CreateAsync(Basar, sellerIq.AsNoTracking(), pageIndex ?? 1, pageSize, Request.Path, GetRoute);

            return Page();
        }

        public IDictionary<string, string> GetItemRoute(Seller seller)
        {
            var route = GetRoute();
            route.Add("sellerId", seller.Id.ToString());
            return route;
        }
    }
}
