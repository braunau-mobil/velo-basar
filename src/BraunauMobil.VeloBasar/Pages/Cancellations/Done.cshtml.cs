using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class DoneParameter
    {
        public int CancellationId { get; set; }
        public int? SaleId { get; set; }
    }
    public class DoneModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public DoneModel(VeloBasarContext context)
        {
            _context = context;
        }

        public ProductsTransaction Cancellation { get; set; }
        [Display(Name = "Auszuzahlender Betrag")]
        [DataType(DataType.Currency)]
        public decimal PayoutAmout { get => Cancellation.GetSum(); }
        public ProductsTransaction Sale { get; set; }

        public async Task OnGetAsync(DoneParameter parameter)
        {
            Cancellation = await _context.Transactions.GetAsync(parameter.CancellationId);
            if (parameter.SaleId != null)
            {
                Sale = await _context.Transactions.GetAsync(parameter.SaleId.Value);
            }
        }
    }
}
