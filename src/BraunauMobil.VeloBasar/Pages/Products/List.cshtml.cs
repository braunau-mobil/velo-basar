using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

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
        public string MyPath => "/Products/List";
        public PaginatedListViewModel<Product> Products { get; set; }
        public StorageState? StorageStateFilter { get; set; }
        public ValueState? ValueStateFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, string currentFilter, string searchString, int? pageIndex, StorageState? storageState, ValueState? valueState)
        {
            await LoadBasarAsync(basarId);
            ViewData["StorageStates"] = GetStorageStates();
            ViewData["ValueStates"] = GetValueStates();
            
            StorageStateFilter = storageState;
            CurrentFilter = searchString;
            ValueStateFilter = valueState;

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

            var productIq = Context.Product.GetMany(searchString, storageState, valueState);

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
        private SelectList GetStorageStates()
        {
            return new SelectList(new[]
            {
                new Tuple<StorageState?, string>(null, "Alle"),
                new Tuple<StorageState?, string>(StorageState.Available, "Verfügbar"),
                new Tuple<StorageState?, string>(StorageState.Sold, "Verkauft"),
                new Tuple<StorageState?, string>(StorageState.Gone, "Verscwunden"),
                new Tuple<StorageState?, string>(StorageState.Locked, "Gesperrt")
            }, "Item1", "Item2");
        }
        private SelectList GetValueStates()
        {
            return new SelectList(new[]
            {
                new Tuple<ValueState?, string>(null, "Alle"),
                new Tuple<ValueState?, string>(ValueState.Settled, "Abgerechnet"),
                new Tuple<ValueState?, string>(ValueState.NotSettled, "Nicht Abgerechnet")
            }, "Item1", "Item2");
        }
    }
}
