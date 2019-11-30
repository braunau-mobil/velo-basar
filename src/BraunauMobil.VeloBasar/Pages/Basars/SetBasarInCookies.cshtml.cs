using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    [Authorize]
    public class SetBasarInCookiesModel : PageModel
    {
        public IActionResult OnGet(int basarId)
        {
            Response.Cookies.SetBasarId(basarId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}