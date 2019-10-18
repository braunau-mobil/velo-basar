using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using Microsoft.Extensions.Localization;
using BraunauMobil.VeloBasar.Resources;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class DoneModel : BasarPageModel
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DoneModel(VeloBasarContext context, IStringLocalizer<SharedResource> localizer) : base(context)
        {
            _localizer = localizer;
        }

        public ProductsTransaction Cancellation { get; set; }
        [Display(Name = "Auszuzahlender Betrag")]
        [DataType(DataType.Currency)]
        public decimal PayoutAmout { get => Cancellation.GetSum(); }
        public ProductsTransaction Sale { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int cancellationId, int? saleId)
        {
            await LoadBasarAsync(basarId);
            Cancellation = await Context.Transactions.GetAsync(cancellationId);
            if (saleId != null)
            {
                Sale = await Context.Transactions.GetAsync(saleId.Value);
            }
            return Page();
        }
        public IDictionary<string, string> GetSaleDocumentRoute()
        {
            var route = GetRoute();
            route.Add("fileId", Sale.DocumentId.ToString());
            return route;
        }
    }
}
