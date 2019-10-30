using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class EditParameter
    {
        public int ProductTypeId { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly VeloBasarContext _context;
        private int _pageIndex;

        public EditModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductType ProductType { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            ProductType = await _context.ProductTypes.GetAsync(parameter.ProductTypeId);
            _pageIndex = parameter.PageIndex;

            if (ProductType == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ProductType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = parameter.PageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
    }
}
