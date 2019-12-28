using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class CreateAndPrintForAcceptanceParameter
    {
        public int AcceptanceNumber { get; set; }
    }
    public class CreateAndPrintForAcceptanceModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionService;

        public CreateAndPrintForAcceptanceModel(IVeloContext context, ITransactionContext transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> OnGetAsync(CreateAndPrintForAcceptanceParameter parameter)
        {
            Contract.Requires(parameter != null);

            var pdf = await _transactionService.CreateLabelsForAcceptanceAsync(_context.Basar, parameter.AcceptanceNumber);
            return File(pdf.Data, pdf.ContentType);
        }
    }
}