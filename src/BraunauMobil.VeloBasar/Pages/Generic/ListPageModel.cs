using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    [Authorize]
    public class ListPageModel<TModel> : BasePageModel<TModel>, IListPageModel, ISearchable where TModel : class, IModel, new()
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
        IEnumerable<IListItem> IListPageModel.Items { get => Items.List; }
        public IPaginatable Paginatable { get => Items; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var iq = _crudContext.GetMany(parameter.SearchString);
            Items = await PaginationHelper.CreateAsync(_veloContext.Basar, iq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);
        }
        public VeloPage CreatePage() => new VeloPage() { Page = CreatePageRoute() };
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
        public VeloPage DeletePage(IListItem listItem)
        {
            if (listItem == null) throw new ArgumentNullException(nameof(listItem));
            if (listItem.Item is TModel item)
            {
                return GetDeletePage(item);
            }
            throw new InvalidOperationException($"Incompatible Item Type");
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
        public VeloPage EditPage(IListItem listItem)
        {
            if (listItem == null) throw new ArgumentNullException(nameof(listItem));
            if (listItem.Item is TModel item)
            {
                return GetEditPage(item);
            }
            throw new InvalidOperationException($"Incompatible Item Type");
        }
        public VeloPage SearchPage() => ListPage(Items.PageIndex, null);

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
        public VeloPage SetStatePage(IListItem listItem, ObjectState stateToSet)
        {
            if (listItem == null) throw new ArgumentNullException(nameof(listItem));
            if (listItem.Item is TModel item)
            {
                return GetSetStatePage(item, stateToSet);
            }
            throw new InvalidOperationException($"Incompatible Item Type");
        }

        public async Task<bool> CanDeleteAsync(TModel item) => await _crudContext.CanDeleteAsync(item);
        public async Task<bool> CanDeleteAsync(IListItem listItem)
        {
            if (listItem == null) throw new ArgumentNullException(nameof(listItem));
            if (listItem.Item is TModel item)
            {
                return await CanDeleteAsync(item);
            }
            throw new InvalidOperationException($"Incompatible Item Type");
        }
    }
}
