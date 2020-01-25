using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class ListParameter : SearchAndPaginationParameter
    {
        public ValueState? ValueState { get; set; }
    }
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly ISellerContext _sellerContext;

        public ListModel(IVeloContext context, ISellerContext sellerContext)
        {
            _context = context;
            _sellerContext = sellerContext;
        }

        public string SearchString { get; set; }
        public PaginatedListViewModel<Seller> Sellers { get;set; }
        public ValueState? ValueStateFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(ListParameter parameter)
        {
            Contract.Requires(parameter != null);

            ViewData["ValueStates"] = GetValueStates();

            SearchString = parameter.SearchString;
            ValueStateFilter = parameter.ValueState;

            if (int.TryParse(parameter.SearchString, out int id))
            {
                if (await _sellerContext.ExistsAsync(id))
                {
                    return this.RedirectToPage<DetailsModel>(new DetailsParameter { SellerId = id });
                }
            }

            var sellerIq = _sellerContext.GetMany(parameter.SearchString, parameter.ValueState);
            Sellers = await PaginationHelper.CreateAsync(_context.Basar, sellerIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);

            return Page();
        }
        public VeloPage GetEditPage(Seller seller)
        {
            Contract.Requires(seller != null);
            return this.GetPage<EditModel>(new EditParameter { SellerId = seller.Id });
        }
        public VeloPage GetDetailsPage(Seller seller)
        {
            Contract.Requires(seller != null);
            return this.GetPage<DetailsModel>(new DetailsParameter { SellerId = seller.Id });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(GetParameter(pageIndex, pageSize));
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(GetParameter(Sellers.PageIndex, null));
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
                ValueState = ValueStateFilter
            };
        }
    }
}
