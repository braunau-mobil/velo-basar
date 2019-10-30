﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace BraunauMobil.VeloBasar.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IVeloContext _context;

        public IndexModel(IVeloContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            //  check if we need initial setup
            if (!_context.Db.IsInitialized())
            {
                return this.RedirectToPage<Setup.IndexModel>();
            }

            if (_context.Basar == null)
            {
                if (_context.IsDebug)
                {
                    return this.RedirectToPage<Setup.DangerZoneModel>();
                }
                throw new NotImplementedException("What should we do in this case??");
            }
            return Page();
        }
    }
}
