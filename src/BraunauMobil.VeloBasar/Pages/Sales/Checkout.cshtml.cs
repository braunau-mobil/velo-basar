using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Sales
{
    public class CheckoutModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public CheckoutModel(IVeloContext context, ITransactionContext transactionContext)
        {
            _context = context;
            _transactionContext = transactionContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var productIds = Request.Cookies.GetCart();
            if (productIds.Count <= 0)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed);
            }

            var sale = await _transactionContext.CheckoutProductsAsync(_context.Basar, productIds);
            
            if (sale == null)
            {
                return this.RedirectToPage<CartModel>(new CartParameter { ShowError = true });
            }
            
            Response.Cookies.ClearCart();
            return this.RedirectToPage<Transactions.DetailsModel>(new Transactions.DetailsParameter { TransactionId = sale.Id, ShowSuccess = true, OpenDocument = true });
        }
    }
}