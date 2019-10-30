using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using System.ComponentModel.DataAnnotations;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class StartModel : PageModel
    {
        private readonly IVeloContext _context;

        public StartModel(IVeloContext context)
        {
            _context = context;
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

            var sale = await _context.Db.Transactions.GetAsync(_context.Basar, TransactionType.Sale, SaleNumber);
            if (sale == null)
            {
                ErrorMessage = _context.Localizer["Es konnte kein Verkauf mit der Nummer {0} gefunden werden", SaleNumber];
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
