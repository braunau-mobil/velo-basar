using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class DangerZoneModel : PageModel
    {
        private readonly VeloBasarContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DangerZoneModel(VeloBasarContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Config = new DataGeneratorConfiguration();
        }

        [BindProperty]
        public DataGeneratorConfiguration Config { get; set; }

#if DEBUG
        public async Task<IActionResult> OnPostAsync()
        {
            var generator = new DataGenerator(_context, _userManager, Config);
            await generator.GenerateAsync();

            return this.RedirectToPage<IndexModel>();
        }
#endif
    }
}
