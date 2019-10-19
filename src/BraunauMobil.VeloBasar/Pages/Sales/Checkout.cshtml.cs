using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CheckoutModel : BasarPageModel
    {
        public CheckoutModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId)
        {
            await LoadBasarAsync(basarId);

            var products = Request.Cookies.GetCart();
            var sale = await Context.CheckoutProductsAsync(Basar, products);
            
            if (sale == null)
            {
                return RedirectToPage("/Sales/Cart", new { basarId, showError = true });
            }
            
            Response.Cookies.ClearCart();
            return RedirectToPage("/Sales/Details", new { basarId, saleId = sale.Id, showSuccess = true, openDocument = true});
        }
    }
}