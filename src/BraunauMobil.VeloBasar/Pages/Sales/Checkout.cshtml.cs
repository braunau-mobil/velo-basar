using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CheckoutModel : PageModel
    {
        private readonly IVeloContext _context;

        public CheckoutModel(IVeloContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var productIds = Request.Cookies.GetCart();
            var printSettings = await _context.Db.GetPrintSettingsAsync();
            var sale = await _context.Db.CheckoutProductsAsync(_context.Basar, printSettings, productIds);
            
            if (sale == null)
            {
                return this.RedirectToPage<CartModel>(new CartParameter { ShowError = true });
            }
            
            Response.Cookies.ClearCart();
            return this.RedirectToPage<DetailsModel>(new DetailsParameter { SaleId = sale.Id, ShowSuccess = true, OpenDocument = true });
        }
    }
}