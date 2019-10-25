using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class StartModel : BasarPageModel
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StartModel(VeloBasarContext context, IStringLocalizer<SharedResource> localizer) : base(context)
        {
            _localizer = localizer;
        }

        [Required]
        [Display(Name = "Verkaufs Nummer")]
        [BindProperty]
        public int SaleNumber { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId)
        {
            await LoadBasarAsync(basarId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int basarId)
        {
            await LoadBasarAsync(basarId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var sale = await Context.Transactions.GetAsync(Basar, Models.TransactionType.Sale, SaleNumber);
            if (sale == null)
            {
                ErrorMessage = _localizer["Es konnte kein Verkauf mit der Nummer {0} gefunden werden", SaleNumber];
                return Page();
            }
            if (!sale.Products.IsAllowed(TransactionType.Cancellation))
            {
                ErrorMessage = _localizer["Es wurden bereits alle Artikel des Verkaufs abgerechnet. Ein Storno ist nicht mehr möglich."];
                return Page();
            }

            return RedirectToPage("/Cancellations/SelectProducts", new { basarId, saleId = sale.Id });
        }
    }
}
