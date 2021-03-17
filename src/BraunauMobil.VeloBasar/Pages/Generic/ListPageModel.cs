using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    [Authorize]
    public class ListPageModel<TModel> : BasePageModel<TModel>, ISearchable where TModel : class, IModel, new()
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
            return new VeloPage
            {
                Page = DeletePageRoute(),
                Parameter = new BasePageParameter
                {
                    Id = item.Id,
                    OriginPageIndex = Items.PageIndex
                }
            };
        }

        public VeloPage GetEditPage(TModel item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return new VeloPage
            {
                Page = EditPageRoute(),
                Parameter = new BasePageParameter
                {
                    Id = item.Id,
                    OriginPageIndex = Items.PageIndex
                }
            };
        }

        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => ListPage(pageIndex, pageSize);
        public VeloPage GetResetPage() => ListPage();
        public VeloPage GetSearchPage() => ListPage(Items.PageIndex, null);
        public VeloPage GetSetStatePage(TModel item, ObjectState stateToSet)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return new VeloPage
            {
                Page = SetStatePageRoute(),
                Parameter = new SetStateParameter
                {
                    Id = item.Id,
                    OriginPageIndex = Items.PageIndex,
                    State = stateToSet
                }
            };
        }

        public async Task<bool> CanDeleteAsync(TModel item) => await _crudContext.CanDeleteAsync(item);
    }
}
