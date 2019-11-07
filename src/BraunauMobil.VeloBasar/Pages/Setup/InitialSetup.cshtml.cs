using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class InitialSetupModel : PageModel
    {
        private readonly VeloBasarContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public InitialSetupModel(VeloBasarContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Config = new InitializationConfiguration();
        }

        [BindProperty]
        public InitializationConfiguration Config
        {
            get;
            set;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.InitializeDatabase(_userManager, Config);

            return this.RedirectToPage<IndexModel>();
        }
    }
}