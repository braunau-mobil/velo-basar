using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.Countries
{
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

        public string SearchString { get; set; }
        public PaginatedListViewModel<Country> Countries { get; set; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var CountryIq = _CountryContext.GetMany(parameter.SearchString);
            Countries = await PaginationHelper.CreateAsync(_context.Basar, CountryIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);
        }
        public VeloPage GetDeletePage(Country item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<DeleteModel>(new DeleteParameter { CountryId = item.Id, PageIndex = Countries.PageIndex });
        }
        public VeloPage GetEditPage(Country item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<EditModel>(new EditParameter { CountryId = item.Id, PageIndex = Countries.PageIndex });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(pageIndex, pageSize);
        public VeloPage GetResetPage() => this.GetPage<ListModel>();
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(Countries.PageIndex, null);
        public async Task<bool> CanDeleteAsync(Country item) => await _CountryContext.CanDeleteAsync(item);
    }
}
