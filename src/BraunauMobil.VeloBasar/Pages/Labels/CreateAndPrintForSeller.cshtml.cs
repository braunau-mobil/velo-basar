using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class CreateAndPrintForSellerParameter
    {
        public int SellerId { get; set; }
    }
    public class CreateAndPrintForSellerModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public CreateAndPrintForSellerModel(IVeloContext context, ITransactionContext transactionContext)
        {
            _context = context;
            _transactionContext = transactionContext;
        }

        public async Task<IActionResult> OnGetAsync(CreateAndPrintForSellerParameter parameter)
        {
            Contract.Requires(parameter != null);

            var file = await _transactionContext.CreateLabelsForSellerAsync(_context.Basar, parameter.SellerId);

            return File(file.Data, file.ContentType);
        }
    }
}