using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Transactions
{
    public class DetailsParameter
    {
        public int TransactionId { get; set; }
        public bool? ShowSuccess { get; set; }
        public bool? OpenDocument { get; set; }
    }
    public class DetailsModel : PageModel
    {
        private readonly ITransactionContext _context;

        public DetailsModel(ITransactionContext context)
        {
            _context = context;
        }

        public TransactionDetailsViewModel TransactionViewModel { get; set; }
        public DetailsParameter Parameter { get; private set; }

        public async Task OnGetAsync(DetailsParameter parameter)
        {
            Contract.Requires(parameter != null);

            Parameter = parameter;
            var transaction = await _context.GetAsync(parameter.TransactionId);
            TransactionViewModel = new TransactionDetailsViewModel(transaction);
        }
    }
}
