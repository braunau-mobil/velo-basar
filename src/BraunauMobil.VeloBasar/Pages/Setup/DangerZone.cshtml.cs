using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class DangerZoneModel : BasarPageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DangerZoneModel(VeloBasarContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            _userManager = userManager;
            Config = new DataGeneratorConfiguration();
        }

        [BindProperty]
        public DataGeneratorConfiguration Config { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var generator = new DataGenerator(Context, _userManager, Config);
            await generator.GenerateAsync();

            return RedirectToPage("/Index");
        }
    }
}