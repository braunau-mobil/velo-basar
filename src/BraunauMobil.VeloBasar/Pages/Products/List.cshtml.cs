using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class ListModel : BasarPageModel, ISearchable
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ListModel(VeloBasarContext context, IStringLocalizer<SharedResource> localizer) : base(context)
        {
            _localizer = localizer;
        }

        public string CurrentFilter { get; set; }

        public PaginatedListViewModel<Product> Products { get; set; }

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

            var productIq = Context.Product.GetMany(searchString);

            var pageSize = 11;
            Products = await PaginatedListViewModel<Product>.CreateAsync(Basar, productIq, pageIndex ?? 1, pageSize, Request.Path, GetRoute, new[]
            {
                new ListCommand<Product>(GetItemDetailsRoute)
                {
                    Text = _localizer["Details"],
                    Page = "/Products/Details"
                },
                new ListCommand<Product>(item => item.Label != null, GetItemDocumentRoute)
                {
                    Text = _localizer["Etikett"],
                    Page = "/ShowFile"
                }
            });
            return Page();
        }

        public IDictionary<string, string> GetItemDetailsRoute(Product product)
        {
            var route = GetRoute();
            route.Add("productId", product.Id.ToString());
            return route;
        }
        public IDictionary<string, string> GetItemDocumentRoute(Product product)
        {
            var route = GetRoute();
            route.Add("fileId", product.Label.Value.ToString());
            return route;
        }
    }
}
