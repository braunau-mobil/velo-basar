using System;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Settlements
{
    public class CreateAndPrintParameter
    {
        public int SellerId { get; set; }
    }
    public class CreateAndPrintModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;
        private readonly IFileStoreContext _fileStoreContext;

        public CreateAndPrintModel(IVeloContext context, ITransactionContext transactionContext, IFileStoreContext fileStoreContext)
        {
            _context = context;
            _transactionContext = transactionContext;
            _fileStoreContext = fileStoreContext;
        }

        public async Task<IActionResult> OnGetAsync(CreateAndPrintParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var settlement = await _transactionContext.SettleSellerAsync(_context.Basar, parameter.SellerId);

            return this.RedirectToPage<Transactions.DetailsModel>(new Transactions.DetailsParameter
            {
                OpenDocument = true,
                ShowChange = true,
                ShowSuccess = true,
                TransactionId = settlement.Id
            });
        }
    }
}