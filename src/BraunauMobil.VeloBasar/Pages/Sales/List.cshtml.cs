using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sales
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
        private const int PageSize = 20;


        public ListModel(IVeloContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<ProductsTransaction> Sales { get; set; }

        public async Task OnGetAsync(ListParameter parameter)
        {
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

            IQueryable<ProductsTransaction> salesIq;

            if (int.TryParse(parameter.SearchString, out int id))
            {
                salesIq = _context.Db.Transactions.Where(t => t.Id == id);
            }
            else if (!string.IsNullOrEmpty(parameter.SearchString))
            {
                salesIq = _context.Db.Transactions.GetMany(TransactionType.Sale, _context.Basar, parameter.SearchString);
            }
            else
            {
                salesIq = _context.Db.Transactions.GetMany(TransactionType.Sale, _context.Basar);
            }

            Sales = await PaginatedListViewModel<ProductsTransaction>.CreateAsync(_context.Basar, salesIq.AsNoTracking(), parameter.PageIndex ?? 1, PageSize, GetPaginationPage);
        }
        public VeloPage GetDetailsPage(ProductsTransaction item) => this.GetPage<DetailsModel>(new DetailsParameter { SaleId = item.Id });
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
    }
}
