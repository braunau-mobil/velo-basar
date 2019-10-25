using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages
{
    public class IndexModel : BasarPageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(VeloBasarContext context, SignInManager<IdentityUser> signInManager) : base(context)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(int? basarId)
        {
            //  check if we need initial setup
            if (!Context.IsInitialized())
            {
                return RedirectToPage("/Setup/Index");
            }

            if (basarId != null)
            {
                return await RedirectToBasar(basarId.Value);
            }

            basarId = Request.Cookies.GetBasarId();
            if (basarId != null && await Context.Basar.ExistsAsync(basarId.Value))
            {
                return await RedirectToBasar(basarId.Value);
            }

            basarId = await Context.Basar.GetUniqueEnabledAsync();
            if (basarId != null)
            {
                return await RedirectToBasar(basarId.Value);
            }

            if (Context.Basar.Any())
            {
                return RedirectToPage("/Basars/Select");
            }

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Basars/Create");
            }

            return RedirectToPage("/Setup/Login");
        }
        private async Task<IActionResult> RedirectToBasar(int basarId)
        {
            await LoadBasarAsync(basarId);
            Response.Cookies.SetBasarId(basarId);
            return Page();
        }
    }
}
