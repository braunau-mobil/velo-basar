using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Brands
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
        private readonly IBrandContext _brandContext;

        public ListModel(IVeloContext context, IBrandContext brandContext)
        {
            _context = context;
            _brandContext = brandContext;
        }

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<Brand> Brands { get; set; }

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

            var brandIq = _brandContext.GetMany(parameter.SearchString);
            var pageSize = 10;
            Brands = await PaginatedListViewModel<Brand>.CreateAsync(_context.Basar, brandIq.AsNoTracking(), parameter.PageIndex ?? 1, pageSize, GetPaginationPage);
        }
        public VeloPage GetDeletePage(Brand item)
        {
            Contract.Requires(item != null);
            return this.GetPage<DeleteModel>(new DeleteParameter { BrandId = item.Id, PageIndex = Brands.PageIndex });
        }
        public VeloPage GetEditPage(Brand item)
        {
            Contract.Requires(item != null);
            return this.GetPage<EditModel>(new EditParameter { BrandId = item.Id, PageIndex = Brands.PageIndex });
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
        public VeloPage GetSetStatePage(Brand item, ObjectState stateToSet)
        {
            Contract.Requires(item != null);
            return this.GetPage<SetStateModel>(new SetStateParameter { BrandId = item.Id, PageIndex = Brands.PageIndex, State = stateToSet });
        }
        public async Task<bool> CanDeleteAsync(Brand item) => await _brandContext.CanDeleteAsync(item);
    }
}
