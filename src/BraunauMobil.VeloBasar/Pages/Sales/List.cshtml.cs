using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class ListModel : BasarPageModel, IPagination
    {
        private const int PageSize = 20;

        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<ProductsTransaction> Sales { get;set; }

        public int PageIndex => Sales.PageIndex;

        public int TotalPages => Sales.TotalPages;

        public bool HasPreviousPage => Sales.HasPreviousPage;

        public bool HasNextPage => Sales.HasNextPage;

        public string MyPath => "/Sales/List";

        public IDictionary<string, string> GetPaginationRoute()
        {
            return GetRoute();
        }

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
                salesIq = Context.GetSales(Basar, searchString);
            }
            else
            {
                salesIq = Context.GetSales(Basar);
            }

            Sales = await PaginatedList<ProductsTransaction>.CreateAsync(salesIq.AsNoTracking(), pageIndex ?? 1, PageSize);
        }
    }
}
