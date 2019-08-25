using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages
{
    public class IndexModel : BasarPageModel
    {
        public IndexModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int? basarId)
        {
            //  check if we need initial setup
            if (!Context.IsInitialized())
            {
                return RedirectToPage("/Setup/Index");
            }

            if (basarId == null)
            {
                var basarIdFromCookieString = Request.Cookies["basarId"];
                if (int.TryParse(basarIdFromCookieString, out int basarIdFromCookie))
                {
                    basarId = basarIdFromCookie;
                }
                else
                {
                    return RedirectToPage("/Basars/SelectOrCreate");
                }
            }

            await LoadBasarAsync(basarId);

            Response.Cookies.Append("basarId", basarId.ToString());

            return Page();
        }
    }
}
