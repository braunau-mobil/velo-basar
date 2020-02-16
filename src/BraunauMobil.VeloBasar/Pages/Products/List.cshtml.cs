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
        public int? BrandId { get; set; }
        public int? ProductTypeId { get; set; }
    }
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly IProductContext _productContext;
        private readonly IBrandContext _brandContext;
        private readonly IProductTypeContext _productTypeContext;

        public ListModel(IVeloContext context, IProductContext productContext, IBrandContext brandContext, IProductTypeContext productTypeContext)
        {
            _context = context;
            _productContext = productContext;
            _brandContext = brandContext;
            _productTypeContext = productTypeContext;
        }

        public string SearchString { get; set; }
        public ProductsViewModel Products { get; set; }
        public StorageState? StorageStateFilter { get; set; }
        public ValueState? ValueStateFilter { get; set; }
        public int? BrandFilter { get; set; }
        public int? ProductTypeFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(ListParameter parameter)
        {
            Contract.Requires(parameter != null);

            ViewData["StorageStates"] = GetStorageStates();
            ViewData["ValueStates"] = GetValueStates();
            ViewData["Brands"] = _brandContext.GetSelectListWithAllItem();
            ViewData["ProductTypes"] = _productTypeContext.GetSelectListWithAllItem();

            BrandFilter = parameter.BrandId;
            ProductTypeFilter = parameter.ProductTypeId;
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

            var productIq = _productContext.GetProductsForBasar(_context.Basar, SearchString, StorageStateFilter, ValueStateFilter, BrandFilter, ProductTypeFilter);
            Products = await ProductsViewModel.CreateAsync(productIq, parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage,
            new[]
            {
                new ListCommand<ProductViewModel>(vm => this.GetPage<DetailsModel>(new DetailsParameter { ProductId = vm.Product.Id }))
                {
                    Text = _context.Localizer["Details"]
                }
            });
            Products.ShowSeller = true;
            return Page();
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(GetParameter(pageIndex, null));
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(GetParameter(pageIndex, pageSize));
        public VeloPage GetResetPage() => this.GetPage<ListModel>();
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
                BrandId = BrandFilter,
                PageIndex = pageIndex,
                PageSize = pageSize,
                ProductTypeId = ProductTypeFilter,
                SearchString = SearchString,
                StorageState = StorageStateFilter,
                ValueState = ValueStateFilter
            };
        }
    }
}
