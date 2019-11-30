using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.DevTools
{
    public class DangerZoneModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DangerZoneModel(IVeloContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Config = new DataGeneratorConfiguration();
        }

        [BindProperty]
        public DataGeneratorConfiguration Config { get; set; }

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
            var generator = new DataGenerator(_context.Db, _userManager, Config);
            await generator.GenerateAsync();

            return this.RedirectToPage<IndexModel>();
        }
    }
}
