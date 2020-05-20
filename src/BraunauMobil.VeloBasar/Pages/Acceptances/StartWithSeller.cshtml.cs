using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class StartWithSellerParameter
    {
        public int SellerId { get; set; }
    }
    public class StartWithSellerModel : PageModel
    {
        public IActionResult OnGet(StartWithSellerParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            Response.Cookies.ClearAcceptanceProducts();
            return this.RedirectToPage<EnterProductsModel>(new EnterProductsParameter { SellerId = parameter.SellerId });
        }
    }
}