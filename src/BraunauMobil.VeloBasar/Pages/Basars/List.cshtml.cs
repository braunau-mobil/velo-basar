using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
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

        public string SearchString { get; set; }
        public PaginatedListViewModel<Basar> Basars { get; set; }

        public async Task<IActionResult> OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var basarIq = _basarContext.GetMany(parameter.SearchString);
            Basars = await PaginationHelper.CreateAsync(_context.Basar, basarIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);

            return Page();
        }
        public VeloPage GetCreatePage() => this.GetPage<CreateModel>();
        public VeloPage GetDeletePage(Basar item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<DeleteModel>(new DeleteParameter { BasarToDeleteId = item.Id, PageIndex = Basars.PageIndex });
        }
        public VeloPage GetDetailsPage(Basar item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<DetailsModel>(new DetailsParameter { BasarId = item.Id });
        }
        public VeloPage GetEditPage(Basar item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<EditModel>(new EditParameter { BasarToEditId = item.Id, PageIndex = Basars.PageIndex });
        }
        public VeloPage GetResetPage() => this.GetPage<ListModel>();
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(Basars.PageIndex, null);
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize = null) => this.GetPage<ListModel>(pageIndex, pageSize);

        public async Task<bool> CanDeleteAsync(Basar item) => await _basarContext.CanDeleteAsync(item);
    }
}
