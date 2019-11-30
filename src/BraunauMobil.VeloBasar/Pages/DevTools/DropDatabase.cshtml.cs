using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.DevTools
{
    public class DropDatabaseModel : PageModel
    {
        private readonly IVeloContext _context;

        public DropDatabaseModel(IVeloContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (!_context.Configuration.DevToolsEnabled())
            {
                return Unauthorized();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!_context.Configuration.DevToolsEnabled())
            {
                return Unauthorized();
            }
            await _context.Db.Database.EnsureDeletedAsync();
            await _context.Db.SaveChangesAsync();
            return this.RedirectToPage<IndexModel>();
        }
    }
}
