using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public ListModel(IVeloContext context, ITransactionContext transactionContext)
        {
            _context = context;
            _transactionContext = transactionContext;
        }

        public string SearchString { get; set; }
        public PaginatedListViewModel<ProductsTransaction> Sales { get; set; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            Contract.Requires(parameter != null);

            SearchString = parameter.SearchString;

            var salesIq = _transactionContext.GetMany(_context.Basar, TransactionType.Sale, parameter.SearchString);
            Sales = await PaginationHelper.CreateAsync(_context.Basar, salesIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);
        }
        public VeloPage GetDetailsPage(ProductsTransaction item)
        {
            Contract.Requires(item != null);
            return this.GetPage<Transactions.DetailsModel>(new Transactions.DetailsParameter { TransactionId = item.Id });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(pageIndex, pageSize);
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(Sales.PageIndex, null);
    }
}
