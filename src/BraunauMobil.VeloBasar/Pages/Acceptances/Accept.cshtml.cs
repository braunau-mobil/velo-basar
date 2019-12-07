using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class AcceptParameter
    {
        public int SellerId { get; set; }
    }
    public class AcceptModel : PageModel
    {
        private readonly IVeloContext _context;

        public AcceptModel(IVeloContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(AcceptParameter parameter)
        {
            Contract.Requires(parameter != null);

            var products = Request.Cookies.GetAcceptanceProducts();
            await _context.Db.ReloadRelationsAsync(products);
            var printSettings = await _context.Db.GetPrintSettingsAsync();
            var acceptance = await _context.Db.AcceptProductsAsync(_context.Basar, parameter.SellerId, printSettings, products);

            Response.Cookies.ClearAcceptanceProducts();
            return this.RedirectToPage<DetailsModel>(new DetailsParameter { AcceptanceId = acceptance.Id, OpenDocument = true, ShowSuccess = true });
        }
    }
}