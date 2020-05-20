using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    [Authorize]
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly IBrandContext _brandContext;

        public ListModel(IVeloContext context, IBrandContext brandContext)
        {
            _context = context;
            _brandContext = brandContext;
        }

        public string SearchString { get; set; }
        public PaginatedListViewModel<Brand> Brands { get; set; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var brandIq = _brandContext.GetMany(parameter.SearchString);
            Brands = await PaginationHelper.CreateAsync(_context.Basar, brandIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);
        }
        public VeloPage GetDeletePage(Brand item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<DeleteModel>(new DeleteParameter { BrandId = item.Id, PageIndex = Brands.PageIndex });
        }
        public VeloPage GetEditPage(Brand item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<EditModel>(new EditParameter { BrandId = item.Id, PageIndex = Brands.PageIndex });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(pageIndex, pageSize);
        public VeloPage GetResetPage() => this.GetPage<ListModel>();
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(Brands.PageIndex, null);
        public VeloPage GetSetStatePage(Brand item, ObjectState stateToSet)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<SetStateModel>(new SetStateParameter { BrandId = item.Id, PageIndex = Brands.PageIndex, State = stateToSet });
        }
        public async Task<bool> CanDeleteAsync(Brand item) => await _brandContext.CanDeleteAsync(item);
    }
}
