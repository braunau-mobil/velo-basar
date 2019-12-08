using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class ListParameter
    {
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageIndex { get; set; }
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

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<Seller> Sellers { get;set; }

        public async Task<IActionResult> OnGetAsync(ListParameter parameter)
        {
            Contract.Requires(parameter != null);

            CurrentFilter = parameter.SearchString;
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
                if (await _sellerContext.ExistsAsync(id))
                {
                    return this.RedirectToPage<DetailsModel>(new DetailsParameter { SellerId = id });
                }
            }

            var sellerIq = _sellerContext.GetMany(parameter.SearchString);

            var pageSize = 20;
            Sellers = await PaginatedListViewModel<Seller>.CreateAsync(_context.Basar, sellerIq.AsNoTracking(), parameter.PageIndex ?? 1, pageSize, GetPaginationPage);

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
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
    }
}
