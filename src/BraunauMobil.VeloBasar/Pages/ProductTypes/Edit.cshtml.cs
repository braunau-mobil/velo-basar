using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class EditParameter
    {
        public int ProductTypeId { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly IProductTypeContext _context;
        private int _pageIndex;

        public EditModel(IProductTypeContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductType ProductType { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            ProductType = await _context.GetAsync(parameter.ProductTypeId);
            _pageIndex = parameter.PageIndex;

            if (ProductType == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateAsync(ProductType);
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new SearchAndPaginationParameter { PageIndex = _pageIndex });
    }
}
