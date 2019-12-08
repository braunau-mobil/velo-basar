using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
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
        private readonly IProductTypeContext _productTypeContext;

        public ListModel(IVeloContext context, IProductTypeContext productTypeContext)
        {
            _context = context;
            _productTypeContext = productTypeContext;
        }

        public string CurrentFilter { get; set; }
        public PaginatedListViewModel<ProductType> ProductTypes { get;set; }

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

            var ProductTypeIq = _productTypeContext.GetMany(parameter.SearchString);
            var pageSize = 10;
            ProductTypes = await PaginatedListViewModel<ProductType>.CreateAsync(_context.Basar, ProductTypeIq.AsNoTracking(), parameter.PageIndex ?? 1, pageSize, GetPaginationPage);
        }
        public VeloPage GetDeletePage(ProductType item)
        {
            Contract.Requires(item != null);
            return this.GetPage<DeleteModel>(new DeleteParameter { PageIndex = ProductTypes.PageIndex, ProductTypeId = item.Id });
        }
        public VeloPage GetEditPage(ProductType item)
        {
            Contract.Requires(item != null);
            return this.GetPage<EditModel>(new EditParameter { PageIndex = ProductTypes.PageIndex, ProductTypeId = item.Id });
        }
        public VeloPage GetPaginationPage(int pageIndex) => this.GetPage<ListModel>(new ListParameter { PageIndex = pageIndex });
        public VeloPage GetSearchPage() => this.GetPage<ListModel>();
        public VeloPage GetSetStatePage(ProductType item, ObjectState stateToSet)
        {
            Contract.Requires(item != null);
            return this.GetPage<SetStateModel>(new SetStateParameter { PageIndex = ProductTypes.PageIndex, ProductTypeId = item.Id, State = stateToSet });
        }
        public async Task<bool> CanDeleteAsync(ProductType item) => await _productTypeContext.CanDeleteAsync(item);
    }
}
