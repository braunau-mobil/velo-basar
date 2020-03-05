using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System.Diagnostics.Contracts;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class DoneParameter
    {
        public int CancellationId { get; set; }
        public int? SaleId { get; set; }
    }
    [Authorize]
    public class DoneModel : PageModel
    {
        private readonly ITransactionContext _transactionContext;
        private readonly ISettingsContext _settingsContext;

        public DoneModel(ITransactionContext context, ISettingsContext settingsContext)
        {
            _transactionContext = context;
            _settingsContext = settingsContext;
        }

        public ProductsTransaction Cancellation { get; set; }
        [Display(Name = "Auszuzahlender Betrag")]
        [DataType(DataType.Currency)]
        public decimal PayoutAmout { get => Cancellation.Products.GetProducts().SumPrice(); }
        public ProductsTransaction Sale { get; set; }
        public ProductsViewModel CancelledProducts { get; private set; }
        public ProductsViewModel SaleProducts { get; private set; }
        public ChangeInfo ChangeInfo { get; private set; }

        public async Task OnGetAsync(DoneParameter parameter)
        {
            Contract.Requires(parameter != null);

            Cancellation = await _transactionContext.GetAsync(parameter.CancellationId);
            if (parameter.SaleId != null)
            {
                Sale = await _transactionContext.GetAsync(parameter.SaleId.Value);
            }
            CancelledProducts = new ProductsViewModel(Cancellation.Products)
            {
                ShowFooter = true,
                FooterValue = Cancellation.GetProductsSum(),
                ShowSeller = false
            };
            if (Sale != null)
            {
                SaleProducts = new ProductsViewModel(Sale.Products)
                {
                    ShowFooter = true,
                    FooterValue = Sale.GetProductsSum(),
                    ShowSeller = false
                };
            }

            var settings = await _settingsContext.GetSettingsAsync();
            ChangeInfo = Cancellation.CalculateChange(0.0m, settings.Nominations);
        }
    }
}
