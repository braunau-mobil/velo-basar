using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    [Authorize]
    public class ListModel : BasarPageModel, IPagination
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<ProductType> ProductTypes { get;set; }

        public int PageIndex => ProductTypes.PageIndex;

        public int TotalPages => ProductTypes.TotalPages;

        public bool HasPreviousPage => ProductTypes.HasPreviousPage;

        public bool HasNextPage => ProductTypes.HasNextPage;

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

            var ProductTypeIq = Context.GetProductTypes(searchString);
            var pageSize = 10;
            ProductTypes = await PaginatedList<ProductType>.CreateAsync(
                ProductTypeIq.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }

        public IDictionary<string, string> GetPaginationRoute()
        {
            return GetRoute();
        }

        public IDictionary<string, string> GetItemRoute(ProductType ProductType)
        {
            var route = GetRoute();
            route.Add("ProductTypeId", ProductType.Id.ToString());
            route.Add("pageIndex", PageIndex.ToString());
            return route;
        }
    }
}
