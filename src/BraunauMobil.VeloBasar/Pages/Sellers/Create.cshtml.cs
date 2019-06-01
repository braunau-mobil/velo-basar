using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class CreateModel : PageModel
    {
        private readonly BraunauMobil.VeloBasar.Data.VeloBasarContext _context;

        public CreateModel(BraunauMobil.VeloBasar.Data.VeloBasarContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Seller Seller { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Seller.Add(Seller);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}