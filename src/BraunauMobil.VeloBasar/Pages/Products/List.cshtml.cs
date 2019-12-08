using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class ListParameter
    {
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageIndex { get; set; }
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

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<Product> Products { get; set; }
        public StorageState? StorageStateFilter { get; set; }
        public ValueState? ValueStateFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(ListParameter parameter)
        {
            ViewData["StorageStates"] = GetStorageStates();
            ViewData["ValueStates"] = GetValueStates();
            
            StorageStateFilter = parameter.StorageState;
            CurrentFilter = parameter.SearchString;
            ValueStateFilter = parameter.ValueState;

            if (parameter.SearchString != null)
            {
                parameter.PageIndex = 1;
            }
            else
            {
                parameter.SearchString = parameter.CurrentFilter;
            }

            CurrentFilter = parameter.SearchString;

            if (int.TryParse(parameter.SearchString, out int id))
            {
                if (await _productContext.ExistsAsync(id))
                {
                    return this.RedirectToPage<DetailsModel>(new DetailsParameter { ProductId = id });
                }
            }

            var productIq = _productContext.GetProductsForBasar(_context.Basar, parameter.SearchString, parameter.StorageState, parameter.ValueState);

            var pageSize = 11;
            Products = await PaginatedListViewModel<Product>.CreateAsync(_context.Basar, productIq, parameter.PageIndex ?? 1, pageSize, GetPaginationPage, new[]
            {
                new ListCommand<Product>(item => this.GetPage<DetailsModel>(new DetailsParameter { ProductId = item.Id }))
                {
                    Text = _context.Localizer["Details"]
                },
                new ListCommand<Product>(item => item.Label != null, item => this.GetPage<ShowFileModel>(new ShowFileParameter { FileId = item.Label.Value }))
                {
                    Text = _context.Localizer["Etikett"]
                }
            });
            return Page();
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
        private SelectList GetStorageStates()
        {
            return new SelectList(new[]
            {
                new Tuple<StorageState?, string>(null, "Alle"),
                new Tuple<StorageState?, string>(StorageState.Available, "Verfügbar"),
                new Tuple<StorageState?, string>(StorageState.Sold, "Verkauft"),
                new Tuple<StorageState?, string>(StorageState.Gone, "Verschwunden"),
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
