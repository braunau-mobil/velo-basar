using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class ListModel : BasarPageModel, IPagination
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<Product> Products { get;set; }

        public int PageIndex => Products.PageIndex;

        public int TotalPages => Products.TotalPages;

        public bool HasPreviousPage => Products.HasPreviousPage;

        public bool HasNextPage => Products.HasNextPage;

        public string MyPath => "/Products/List";

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
                if (await Context.Product.ExistsAsync(id))
                {
                    return RedirectToPage("/Products/Details", new { basarId, productId = id });
                }
            }

            var productIq = Context.GetProducts(searchString);

            var pageSize = 11;
            Products = await PaginatedList<Product>.CreateAsync(
                productIq.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }

        public IDictionary<string, string> GetItemRoute(Product product)
        {
            var route = GetRoute();
            route.Add("productId", product.Id.ToString());
            return route;
        }

        public IDictionary<string, string> GetPaginationRoute()
        {
            return GetRoute();
        }
    }
}
