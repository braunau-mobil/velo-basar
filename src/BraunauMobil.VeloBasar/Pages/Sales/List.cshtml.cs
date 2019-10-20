using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.ViewModels;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class ListModel : BasarPageModel, ISearchable
    {
        private const int PageSize = 20;

        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedListViewModel<ProductsTransaction> Sales { get;set; }

        public string MyPath => "/Sales/List";

        public async Task OnGetAsync(int? basarId, string currentFilter, string searchString, int? pageIndex)
        {
            await LoadBasarAsync(basarId);

            CurrentFilter = searchString;
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<ProductsTransaction> salesIq;

            if (int.TryParse(searchString, out int id))
            {
                salesIq = Context.Transactions.Where(t => t.Id == id);
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                salesIq = Context.Transactions.GetMany(TransactionType.Sale, Basar, searchString);
            }
            else
            {
                salesIq = Context.Transactions.GetMany(TransactionType.Sale, Basar);
            }

            Sales = await PaginatedListViewModel<ProductsTransaction>.CreateAsync(Basar, salesIq.AsNoTracking(), pageIndex ?? 1, PageSize, Request.Path, GetRoute);
        }
    }
}
