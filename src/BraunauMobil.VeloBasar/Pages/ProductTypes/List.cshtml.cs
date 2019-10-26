using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    [Authorize]
    public class ListModel : BasarPageModel, ISearchable
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedListViewModel<ProductType> ProductTypes { get;set; }

        public string MyPath => "/ProductTypes/List";

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

            var ProductTypeIq = Context.ProductTypes.GetMany(searchString);
            var pageSize = 10;
            ProductTypes = await PaginatedListViewModel<ProductType>.CreateAsync(Basar, ProductTypeIq.AsNoTracking(), pageIndex ?? 1, pageSize, Request.Path, GetRoute);

            return Page();
        }

        public IDictionary<string, string> GetItemRoute(ProductType ProductType, ModelState? statusToSet = null)
        {
            var route = GetRoute();
            route.Add("ProductTypeId", ProductType.Id.ToString());
            route.Add("pageIndex", ProductTypes.PageIndex.ToString());
            if (statusToSet != null)
            {
                route.Add("status", statusToSet.ToString());
            }
            return route;
        }
        public async Task<bool> CanDeleteAsync(ProductType item)
        {
            return await Context.CanDeleteProductTypeAsync(item);
        }
    }
}
