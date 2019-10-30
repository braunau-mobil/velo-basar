using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class ClearCartModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.Cookies.ClearCart();
            return this.RedirectToPage<CartModel>();
        }
    }
}