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
        public async Task<IActionResult> OnGetAsync(StartWithSellerParameter parameter)
        {
            Response.Cookies.ClearAcceptanceProducts();
            return this.RedirectToPage<EnterProductsModel>(new EnterProductsParameter { SellerId = parameter.SellerId });
        }
    }
}