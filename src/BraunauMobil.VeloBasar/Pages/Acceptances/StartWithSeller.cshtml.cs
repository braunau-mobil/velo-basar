using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class StartWithSeller : BasarPageModel
    {
        public StartWithSeller(VeloBasarContext context) : base(context)
        {
        }

        public IActionResult OnGet(int basarId, int sellerId)
        {
            Response.Cookies.ClearAcceptanceProducts();
            return RedirectToPage("/Acceptances/EnterProducts", new { basarId, sellerId });
        }
    }
}