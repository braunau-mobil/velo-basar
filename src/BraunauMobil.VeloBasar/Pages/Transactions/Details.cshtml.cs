using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Transactions
{
    public class DetailsParameter
    {
        public int TransactionId { get; set; }
        public bool? ShowChange { get; set; }
        public bool? ShowSuccess { get; set; }
        public bool? OpenDocument { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly ITransactionContext _transactionContext;
        private readonly ISettingsContext _settingsContext;

        public DetailsModel(ITransactionContext context, ISettingsContext settingsContext)
        {
            _transactionContext = context;
            _settingsContext = settingsContext;
        }

        public TransactionDetailsViewModel TransactionViewModel { get; set; }
        public DetailsParameter Parameter { get; private set; }

        [BindProperty]
        [DataType(DataType.Currency)]
        [Display(Name = "Erhalten:")]
        [Range(typeof(decimal), "0.01", "1000000000.000", ErrorMessage = "Bitte einen Preis größer 0,01 eingeben.", ParseLimitsInInvariantCulture = true)]
        public decimal AmountGiven { get; set; }
        public bool CanInputAmountGiven { get => TransactionViewModel.Transaction.Type == TransactionType.Sale; }

        public ChangeInfo ChangeInfo { get; set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            await LoadTransactionAsync(parameter);
        }
        public async Task OnPostAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            parameter.ShowSuccess = false;
            parameter.OpenDocument = false;

            await LoadTransactionAsync(parameter);
        }
        public VeloPage GetChangePage()
        {
            return this.GetPage<DetailsModel>(new DetailsParameter
            {
                OpenDocument = false,
                ShowChange = true,
                ShowSuccess = false,
                TransactionId = Parameter.TransactionId
            });
        }

        private async Task LoadTransactionAsync(DetailsParameter parameter)
        {
            Parameter = parameter;
            var transaction = await _transactionContext.GetAsync(parameter.TransactionId);
            TransactionViewModel = new TransactionDetailsViewModel(transaction);

            var settings = await _settingsContext.GetSettingsAsync();
            ChangeInfo = transaction.CalculateChange(AmountGiven, settings.Nominations);
        }
    }
}
