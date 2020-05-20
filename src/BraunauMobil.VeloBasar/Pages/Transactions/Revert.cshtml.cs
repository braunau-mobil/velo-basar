using System;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Transactions
{
    public class RevertParameter
    {
        public int TransactionId { get; set; }
    }
    public class RevertModel : PageModel
    {
        private readonly ITransactionContext _transactionContext;

        public RevertModel(ITransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        public TransactionDetailsViewModel TransactionViewModel { get; private set; }

        public async Task<IActionResult> OnGetAsync(RevertParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var transaction = await _transactionContext.GetAsync(parameter.TransactionId);
            if (transaction.CanRevert())
            {
                TransactionViewModel = new TransactionDetailsViewModel(transaction);
                return Page();
            }
            return Forbid();

        }
        public async Task<IActionResult> OnPostAsync(RevertParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var transaction = await _transactionContext.GetAsync(parameter.TransactionId);
            if (transaction.CanRevert())
            {
                await _transactionContext.RevertAsync(transaction);
                return this.RedirectToPage<Sellers.DetailsModel>(new Sellers.DetailsParameter { SellerId = transaction.SellerId.Value });
            }
            return Forbid();
        }
        public object GetPostParameter() => new RevertParameter { TransactionId = TransactionViewModel.Transaction.Id };
    }
}