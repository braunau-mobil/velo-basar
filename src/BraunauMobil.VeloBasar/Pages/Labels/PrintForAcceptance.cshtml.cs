using System;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class PrintForAcceptanceParameter
    {
        public int AcceptanceNumber { get; set; }
    }
    public class PrintForAcceptanceModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionService;
        private readonly IFileStoreContext _fileStoreContext;

        public PrintForAcceptanceModel(IVeloContext context, ITransactionContext transactionService, IFileStoreContext fileStoreContext)
        {
            _context = context;
            _transactionService = transactionService;
            _fileStoreContext = fileStoreContext;
        }

        public async Task<IActionResult> OnGetAsync(PrintForAcceptanceParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var acceptance = await _transactionService.GetAsync(_context.Basar, TransactionType.Acceptance, parameter.AcceptanceNumber);
            var file = await _fileStoreContext.GetProductLabelsAndCombineToOnePdfAsync(acceptance.Products.GetProducts());

            return File(file.Data, file.ContentType);
        }
    }
}