using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class ListParameter : SearchAndPaginationParameter
    {
        public StorageState? StorageState { get; set; }
        public ValueState? ValueState { get; set; }
    }
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly IProductContext _productContext;

        public ListModel(IVeloContext context, IProductContext productContext)
        {
            _context = context;
            _productContext = productContext;
        }

        public string SearchString { get; set; }
        public ProductsViewModel Products { get; set; }
        public StorageState? StorageStateFilter { get; set; }
        public ValueState? ValueStateFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(ListParameter parameter)
        {
            Contract.Requires(parameter != null);

            ViewData["StorageStates"] = GetStorageStates();
            ViewData["ValueStates"] = GetValueStates();
            
            StorageStateFilter = parameter.StorageState;
            SearchString = parameter.SearchString;
            ValueStateFilter = parameter.ValueState;

            if (int.TryParse(parameter.SearchString, out int id))
            {
                if (await _productContext.ExistsAsync(id))
                {
                    return this.RedirectToPage<DetailsModel>(new DetailsParameter { ProductId = id });
                }
            }

            var productIq = _productContext.GetProductsForBasar(_context.Basar, SearchString, StorageStateFilter, ValueStateFilter);
            Products = await ProductsViewModel.CreateAsync(productIq, parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage,
            new[]
            {
                new ListCommand<ProductViewModel>(vm => this.GetPage<DetailsModel>(new DetailsParameter { ProductId = vm.Product.Id }))
                {
                    Text = _context.Localizer["Details"]
                },
                new ListCommand<ProductViewModel>(vm => vm.Product.Label != null, vm => this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = vm.Product.Label.Value }))
                {
                    Text = _context.Localizer["Etikett"]
                }
            });
            Products.ShowSeller = true;
            return Page();
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(GetParameter(pageIndex, null));
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(GetParameter(pageIndex, pageSize));
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(GetParameter(Products.PageIndex, null));
        private SelectList GetStorageStates()
        {
            return new SelectList(new[]
            {
                new Tuple<StorageState?, string>(null, _context.Localizer["Alle"]),
                new Tuple<StorageState?, string>(StorageState.Available, _context.Localizer["Verfügbar"]),
                new Tuple<StorageState?, string>(StorageState.Sold, _context.Localizer["Verkauft"]),
                new Tuple<StorageState?, string>(StorageState.Gone, _context.Localizer["Verschwunden"]),
                new Tuple<StorageState?, string>(StorageState.Locked, _context.Localizer["Gesperrt"])
            }, "Item1", "Item2");
        }
        private SelectList GetValueStates()
        {
            return new SelectList(new[]
            {
                new Tuple<ValueState?, string>(null, _context.Localizer["Alle"]),
                new Tuple<ValueState?, string>(ValueState.Settled, _context.Localizer["Abgerechnet"]),
                new Tuple<ValueState?, string>(ValueState.NotSettled, _context.Localizer["Nicht Abgerechnet"])
            }, "Item1", "Item2");
        }
        private ListParameter GetParameter(int pageIndex, int? pageSize)
        {
            return new ListParameter
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchString = SearchString,
                StorageState = StorageStateFilter,
                ValueState = ValueStateFilter
            };
        }
    }
}
