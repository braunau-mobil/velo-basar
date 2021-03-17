using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class ListPageModel<TModel, TDeletePageModel, TEditPageModel, TListPageModel, TSetStateModel> : PageModel, ISearchable where TModel : class, IModel, new() where TDeletePageModel : PageModel where TEditPageModel : PageModel where TListPageModel : PageModel, ISearchable where TSetStateModel : PageModel
    {
        private readonly IVeloContext _veloContext;
        private readonly ICrudContext<TModel> _crudContext;

        public ListPageModel(IVeloContext veloContex, ICrudContext<TModel> crudContext)
        {
            _veloContext = veloContex;
            _crudContext = crudContext;
        }

        public string SearchString { get; set; }
        public PaginatedListViewModel<TModel> Items { get; set; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var iq = _crudContext.GetMany(parameter.SearchString);
            Items = await PaginationHelper.CreateAsync(_veloContext.Basar, iq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);
        }
        public VeloPage GetDeletePage(TModel item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<TDeletePageModel>(new DeleteParameter { Id = item.Id, PageIndex = Items.PageIndex });
        }
        public VeloPage GetEditPage(TModel item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<TEditPageModel>(new EditParameter { Id = item.Id, PageIndex = Items.PageIndex });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<TListPageModel>(pageIndex, pageSize);
        public VeloPage GetResetPage() => this.GetPage<TListPageModel>();
        public VeloPage GetSearchPage() => this.GetPage<TListPageModel>(Items.PageIndex, null);
        public VeloPage GetSetStatePage(TModel item, ObjectState stateToSet)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<TSetStateModel>(new SetStateParameter { Id = item.Id, PageIndex = Items.PageIndex, State = stateToSet });
        }
        public async Task<bool> CanDeleteAsync(TModel item) => await _crudContext.CanDeleteAsync(item);
    }
}
