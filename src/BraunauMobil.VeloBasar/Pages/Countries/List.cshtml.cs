using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Countries
{
    public class ListParameter
    {
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageIndex { get; set; }
    }
    [Authorize]
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly ICountryContext _CountryContext;

        public ListModel(IVeloContext context, ICountryContext CountryContext)
        {
            _context = context;
            _CountryContext = CountryContext;
        }

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<Country> Countries { get; set; }

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

            var CountryIq = _CountryContext.GetMany(parameter.SearchString);
            var pageSize = 10;
            Countries = await PaginationHelper.CreateAsync(_context.Basar, CountryIq.AsNoTracking(), parameter.PageIndex ?? 1, pageSize, GetPaginationPage);
        }
        public VeloPage GetDeletePage(Country item)
        {
            Contract.Requires(item != null);
            return this.GetPage<DeleteModel>(new DeleteParameter { CountryId = item.Id, PageIndex = Countries.PageIndex });
        }
        public VeloPage GetEditPage(Country item)
        {
            Contract.Requires(item != null);
            return this.GetPage<EditModel>(new EditParameter { CountryId = item.Id, PageIndex = Countries.PageIndex });
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
        public async Task<bool> CanDeleteAsync(Country item) => await _CountryContext.CanDeleteAsync(item);
    }
}
