using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    [Authorize]
    public class ListModel : BasarPageModel, IPagination
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<Brand> Brands { get;set; }

        public int PageIndex => Brands.PageIndex;

        public int TotalPages => Brands.TotalPages;

        public bool HasPreviousPage => Brands.HasPreviousPage;

        public bool HasNextPage => Brands.HasNextPage;

        public string MyPath => "/Brands/List";

        public async Task<IActionResult> OnGetAsync(string currentFilter, string searchString, int? pageIndex, int? basarId)
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

            var brandIq = Context.GetBrands(searchString);
            var pageSize = 10;
            Brands = await PaginatedList<Brand>.CreateAsync(
                brandIq.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }
        public IDictionary<string, string> GetPaginationRoute()
        {
            return GetRoute();
        }
        public IDictionary<string, string> GetItemRoute(Brand brand, ModelStatus? statusToSet = null)
        {
            var route = GetRoute();
            route.Add("brandId", brand.Id.ToString());
            route.Add("pageIndex", PageIndex.ToString());
            if (statusToSet != null)
            {
                route.Add("status", statusToSet.ToString());
            }
            return route;
        }
        public async Task<bool> CanDeleteAsync(Brand item)
        {
            return await Context.CanDeleteBrandAsync(item);
        }
    }
}
