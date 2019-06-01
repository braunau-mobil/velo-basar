using System;
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
    public class IndexModel : PageModel
    {
        private readonly BraunauMobil.VeloBasar.Data.VeloBasarContext _context;

        public IndexModel(BraunauMobil.VeloBasar.Data.VeloBasarContext context)
        {
            _context = context;
        }

        public IList<Acceptance> Acceptance { get;set; }

        public async Task OnGetAsync()
        {
            Acceptance = await _context.Acceptance
                .Include(a => a.Basar)
                .Include(a => a.Seller).ToListAsync();
        }
    }
}
