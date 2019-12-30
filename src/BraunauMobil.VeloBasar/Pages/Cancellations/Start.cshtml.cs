using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Linq;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class StartModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public StartModel(IVeloContext context, ITransactionContext transactionContext)
        {
            _context = context;
            _transactionContext = transactionContext;
        }

        [Required]
        [Display(Name = "Verkaufs Nummer")]
        [BindProperty]
        public int SaleNumber { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var sale = await _transactionContext.GetAsync(_context.Basar, TransactionType.Sale, SaleNumber);
            if (sale == null)
            {
                ErrorMessage = _context.Localizer["Es konnte kein Verkauf mit der Nummer {0} gefunden werden", SaleNumber];
                return Page();
            }
            if (!sale.Products.Any())
            {
                ErrorMessage = _context.Localizer["Es wurden bereits alle Artikel des Verkaufs storniert."];
                return Page();
            }
            if (!sale.Products.IsAllowed(TransactionType.Cancellation))
            {
                ErrorMessage = _context.Localizer["Es wurden bereits alle Artikel des Verkaufs abgerechnet. Ein Storno ist nicht mehr möglich."];
                return Page();
            }

            return this.RedirectToPage<SelectProductsModel>(new SelectProductsParameter { SaleId = sale.Id });
        }
    }
}
