using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.DevTools
{
    public class DropDatabaseModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly VeloRepository _db;

        public DropDatabaseModel(IVeloContext context, VeloRepository db)
        {
            _context = context;
            _db = db;
        }

        public IActionResult OnGet()
        {
            if (!_context.DevToolsEnabled())
            {
                return Unauthorized();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!_context.DevToolsEnabled())
            {
                return Unauthorized();
            }
            await _db.Database.EnsureDeletedAsync();
            await _db.SaveChangesAsync();
            return this.RedirectToPage<IndexModel>();
        }
    }
}
