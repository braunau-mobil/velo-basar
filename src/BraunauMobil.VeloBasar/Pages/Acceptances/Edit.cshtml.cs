using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class EditModel : PageModel
    {
        private readonly BraunauMobil.VeloBasar.Data.VeloBasarContext _context;

        public EditModel(BraunauMobil.VeloBasar.Data.VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Acceptance Acceptance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Acceptance = await _context.Acceptance
                .Include(a => a.Basar)
                .Include(a => a.Seller).FirstOrDefaultAsync(m => m.Id == id);

            if (Acceptance == null)
            {
                return NotFound();
            }
           ViewData["BasarId"] = new SelectList(_context.Basar, "Id", "Id");
           ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Acceptance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcceptanceExists(Acceptance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AcceptanceExists(int id)
        {
            return _context.Acceptance.Any(e => e.Id == id);
        }
    }
}
