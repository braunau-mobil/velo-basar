using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    //  @todo Schön machen
    public class SelectModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public SelectModel(VeloBasarContext context)
        {
            _context = context;
        }

        public IList<Basar> Basars { get; set; }

        public async Task OnGetAsync()
        {
            Basars = await _context.Basar.GetEnabled().ToListAsync();
        }
    }
}
