using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class CreateModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public CreateModel(VeloBasarContext context)
        {
            _context = context;
            BasarToCreate = new Basar
            {
                Date = DateTime.Now
            };
        }

        [BindProperty]
        public Basar BasarToCreate { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.CreateBasarAsync(BasarToCreate);

            return this.RedirectToPage<ListModel>();
        }
    }
}