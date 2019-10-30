using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class EditParameter
    {
        public int BrandId { get; set; }
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
        public Brand Brand { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            Brand = await _context.Brand.GetAsync(parameter.BrandId);
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

            _context.Attach(Brand).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return this.RedirectToPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<ListModel>(new ListParameter { PageIndex = _pageIndex });
    }
}
