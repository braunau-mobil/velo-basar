using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar
{
    public class BasarPageModel : PageModel
    {
        public BasarPageModel(VeloBasarContext context)
        {
            Context = context;
        }

        protected VeloBasarContext Context { get; }

        [BindProperty]
        public Basar Basar { get; set; }

        public virtual async Task<IActionResult> OnGetAsync(int? id)
        {
#if DEBUG
            id = 1;
#endif
            if (id != null)
            {
                Basar = await Context.Basar.FirstOrDefaultAsync(m => m.Id == id);
            }
            return Page();
        }
    }
}
