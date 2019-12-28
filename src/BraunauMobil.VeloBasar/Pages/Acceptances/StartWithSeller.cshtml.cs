using System.Diagnostics.Contracts;
using System.Threading.Tasks;
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
            Contract.Requires(parameter != null);

            Response.Cookies.ClearAcceptanceProducts();
            return this.RedirectToPage<EnterProductsModel>(new EnterProductsParameter { SellerId = parameter.SellerId });
        }
    }
}