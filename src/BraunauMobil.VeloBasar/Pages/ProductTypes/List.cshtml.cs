using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    [Authorize]
    public class ListModel : PageModel, ISearchable
    {
        private readonly IVeloContext _context;
        private readonly IProductTypeContext _productTypeContext;

        public ListModel(IVeloContext context, IProductTypeContext productTypeContext)
        {
            _context = context;
            _productTypeContext = productTypeContext;
        }

        public string SearchString { get; set; }
        public PaginatedListViewModel<ProductType> ProductTypes { get;set; }

        public async Task OnGetAsync(SearchAndPaginationParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            SearchString = parameter.SearchString;

            var ProductTypeIq = _productTypeContext.GetMany(parameter.SearchString);
            ProductTypes = await PaginationHelper.CreateAsync(_context.Basar, ProductTypeIq.AsNoTracking(), parameter.GetPageIndex(), parameter.GetPageSize(this), GetPaginationPage);
        }
        public VeloPage GetDeletePage(ProductType item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<DeleteModel>(new DeleteParameter { PageIndex = ProductTypes.PageIndex, ProductTypeId = item.Id });
        }
        public VeloPage GetEditPage(ProductType item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<EditModel>(new EditParameter { PageIndex = ProductTypes.PageIndex, ProductTypeId = item.Id });
        }
        public VeloPage GetPaginationPage(int pageIndex, int? pageSize) => this.GetPage<ListModel>(pageIndex, pageSize);
        public VeloPage GetResetPage() => this.GetPage<ListModel>();
        public VeloPage GetSearchPage() => this.GetPage<ListModel>(ProductTypes.PageIndex, null);
        public VeloPage GetSetStatePage(ProductType item, ObjectState stateToSet)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return this.GetPage<SetStateModel>(new SetStateParameter { PageIndex = ProductTypes.PageIndex, ProductTypeId = item.Id, State = stateToSet });
        }
        public async Task<bool> CanDeleteAsync(ProductType item) => await _productTypeContext.CanDeleteAsync(item);
    }
}
