using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class IndexModel : BasarPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(VeloBasarContext context, UserManager<IdentityUser> userManager) : base(context)
        {
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
            await Context.InitializeDatabase(_userManager, Config);

            return RedirectToPage("/Index");
        }
    }
}