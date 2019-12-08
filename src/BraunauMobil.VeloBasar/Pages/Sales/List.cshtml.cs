using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

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
        private const int PageSize = 20;
        
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public ListModel(IVeloContext context, ITransactionContext transactionContext)
        {
            _context = context;
            _transactionContext = transactionContext;
        }

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<ProductsTransaction> Sales { get; set; }

        public async Task OnGetAsync(ListParameter parameter)
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

            var salesIq = _transactionContext.GetMany(_context.Basar, TransactionType.Sale, parameter.SearchString);
            Sales = await PaginatedListViewModel<ProductsTransaction>.CreateAsync(_context.Basar, salesIq.AsNoTracking(), parameter.PageIndex ?? 1, PageSize, GetPaginationPage);
        }
        public VeloPage GetDetailsPage(ProductsTransaction item)
        {
            Contract.Requires(item != null);
            return this.GetPage<DetailsModel>(new DetailsParameter { SaleId = item.Id });
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
    }
}
