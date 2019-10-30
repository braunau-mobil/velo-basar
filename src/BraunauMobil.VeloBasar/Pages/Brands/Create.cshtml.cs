using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class CreateModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public CreateModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Brand Brand { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.CreateBrand(Brand);
            return this.RedirectToPage<ListModel>();
        }
    }
}