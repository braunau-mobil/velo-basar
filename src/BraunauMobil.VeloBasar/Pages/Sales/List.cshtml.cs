using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

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
        public TransactionsViewModel Sales { get; set; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var salesIq = _transactionContext.GetMany(_context.Basar, TransactionType.Sale, parameter.SearchString);
            Sales = await TransactionsViewModel.CreateAsync(salesIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage, new[]
            {
                new ListCommand<TransactionItemViewModel>(GetTransactionDetailsPage)
                {
                    Text  = _context.Localizer["Details"]
                }
            });
            Sales.ShowDocumentLink = true;
        }
        public VeloPage GetDetailsPage(ProductsTransaction item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<Transactions.DetailsModel>(new Transactions.DetailsParameter { TransactionId = item.Id });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(pageIndex, pageSize);
        public VeloPage GetResetPage() => this.GetPage<ListModel>();
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(Sales.PageIndex, null);
        private VeloPage GetTransactionDetailsPage(TransactionItemViewModel viewModel) => this.GetPage<Transactions.DetailsModel>(new Transactions.DetailsParameter { TransactionId = viewModel.Transaction.Id });
    }
}
