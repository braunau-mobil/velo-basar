﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class DeleteModel : PageModel
    {
        private readonly BraunauMobil.VeloBasar.Data.VeloBasarContext _context;

        public DeleteModel(BraunauMobil.VeloBasar.Data.VeloBasarContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Acceptance = await _context.Acceptance.FindAsync(id);

            if (Acceptance != null)
            {
                _context.Acceptance.Remove(Acceptance);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}