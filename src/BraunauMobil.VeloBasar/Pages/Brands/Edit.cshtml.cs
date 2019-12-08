using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class EditParameter
    {
        public int BrandId { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel : PageModel
    {
        private readonly IBrandContext _context;
        private int _pageIndex;

        public EditModel(IBrandContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Brand Brand { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            Brand = await _context.GetAsync(parameter.BrandId);
            _pageIndex = parameter.PageIndex;

            if (Brand == null)
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

            await _context.UpdateAsync(Brand);
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
    }
}
