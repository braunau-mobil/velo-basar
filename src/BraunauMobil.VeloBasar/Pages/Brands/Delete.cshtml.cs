using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class DeleteModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public DeleteModel(VeloBasarContext context) 
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int brandId, int pageIndex)
        {
            if (await _context.ExistsBrand(brandId))
            {
                await _context.DeleteBrand(brandId);
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/Brands/List", new { pageIndex });
        }
    }
}
