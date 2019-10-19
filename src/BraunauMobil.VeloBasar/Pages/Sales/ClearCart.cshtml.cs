using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class ClearCartModel : BasarPageModel
    {
        public ClearCartModel(VeloBasarContext context) : base(context)
        {
        }

        public IActionResult OnGet(int basarId)
        {
            Response.Cookies.ClearCart();
            return RedirectToPage("/Sales/Cart", new { basarId });
        }
    }
}