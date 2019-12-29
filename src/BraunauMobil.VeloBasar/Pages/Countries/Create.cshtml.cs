using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Countries
{
    public class CreateModel : PageModel
    {
        private readonly ICountryContext _context;

        public CreateModel(ICountryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Country Country { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.CreateAsync(Country);
            return this.RedirectToPage<ListModel>();
        }
    }
}