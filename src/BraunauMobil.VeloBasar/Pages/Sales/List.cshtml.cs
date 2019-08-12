using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using System.Linq;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class ListModel : BasarPageModel
    {
        private const int PageSize = 20;

        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedList<Sale> Sales { get;set; }

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

            var salesIq = from s in Context.Sale select s;

            if (int.TryParse(searchString, out int id))
            {
                salesIq = salesIq.Where(s => s.Id == id);
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                //salesIq = salesIq.Where(s => s.FirstName.Contains(searchString, System.StringComparison.InvariantCultureIgnoreCase) ||s.LastName.Contains(searchString, System.StringComparison.InvariantCultureIgnoreCase));
            }

            Sales = await PaginatedList<Sale>.CreateAsync(salesIq.AsNoTracking(), pageIndex ?? 1, PageSize);
        }
    }
}
