using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class DeleteModel : PageModel
    {
        private readonly BraunauMobil.VeloBasar.Data.VeloBasarContext _context;

        public DeleteModel(BraunauMobil.VeloBasar.Data.VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Basar Basar { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Basar = await _context.Basar.FirstOrDefaultAsync(m => m.Id == id);

            if (Basar == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Basar = await _context.Basar.FindAsync(id);

            if (Basar != null)
            {
                _context.Basar.Remove(Basar);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
