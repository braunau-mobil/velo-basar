using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.Basars
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
        private readonly IBasarContext _basarContext;

        public ListModel(IVeloContext context, IBasarContext basarContext)
        {
            _context = context;
            _basarContext = basarContext;
        }

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<Basar> Basars { get;set; }

        public async Task<IActionResult> OnGetAsync(ListParameter parameter)
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

            var basarIq = _basarContext.GetMany(parameter.SearchString);
            var pageSize = 10;
            Basars = await PaginatedListViewModel<Basar>.CreateAsync(_context.Basar, basarIq.AsNoTracking(), parameter.PageIndex ?? 1, pageSize, GetPaginationPage);

            return Page();
        }
        public VeloPage GetCreatePage() => this.GetPage<CreateModel>();
        public VeloPage GetDeletePage(Basar item) => this.GetPage<DeleteModel>(new DeleteParameter { BasarToDeleteId = item.Id, PageIndex = Basars.PageIndex });
        public VeloPage GetDetailsPage(Basar item)
        {
            Contract.Requires(item != null);
            return this.GetPage<DetailsModel>(new DetailsParameter { BasarId = item.Id });
        }
        public VeloPage GetEditPage(Basar item)
        {
            Contract.Requires(item != null);
            return this.GetPage<EditModel>(new EditParameter { BasarToEditId = item.Id, PageIndex = Basars.PageIndex });
        }
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSetStatePage(Basar item, bool stateToSet)
        {
            //  asp-page="/Basars/SetState" asp-all-route-data="@Model.GetSetStateRoute(itemViewModel.Item, !itemViewModel.Item.IsLocked)"
            throw new System.NotImplementedException();
        }

        public async Task<bool> CanDeleteAsync(Basar item) => await _basarContext.CanDeleteAsync(item);
    }
}
