using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    [Authorize]
    public class ListModel : BasarPageModel, ISearchable
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedListViewModel<Brand> Brands { get;set; }

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

            var brandIq = Context.Brand.GetMany(searchString);
            var pageSize = 10;
            Brands = await PaginatedListViewModel<Brand>.CreateAsync(Basar, brandIq.AsNoTracking(), pageIndex ?? 1, pageSize, Request.Path, GetRoute);

            return Page();
        }
        public IDictionary<string, string> GetItemRoute(Brand brand, ObjectState? stateToSet = null)
        {
            var route = GetRoute();
            route.Add("brandId", brand.Id.ToString());
            route.Add("pageIndex", Brands.PageIndex.ToString());
            if (stateToSet != null)
            {
                route.Add("state", stateToSet.ToString());
            }
            return route;
        }
        public async Task<bool> CanDeleteAsync(Brand item)
        {
            return await Context.CanDeleteBrandAsync(item);
        }
    }
}
