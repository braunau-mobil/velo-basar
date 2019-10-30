using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class CreateModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public CreateModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductType ProductType { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.CreateProductType(ProductType);
            return this.RedirectToPage<ListModel>();
        }
    }
}