using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class SelectAcceptanceParameter
    {
        public int? AcceptanceNumber { get; set; }
        public bool OpenPdf { get; set; }
    }

    public class SelectAcceptanceModel : PageModel
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IVeloContext _context;
        private readonly ITransactionContext _transactionContext;

        public SelectAcceptanceModel(IVeloContext context, LinkGenerator linkGenerator, ITransactionContext transactionContext)
        {
            _context = context;
            _linkGenerator = linkGenerator;
            _transactionContext = transactionContext;
        }

        [BindProperty]
        public int AcceptanceNumber { get; set; }
        public bool NumberNotFound { get; set; }
        public bool OpenPdf { get; set; }

        public async Task<IActionResult> OnGetAsync(SelectAcceptanceParameter parameter)
        {
            if (parameter.AcceptanceNumber == null)
            {
                return Page();
            }

            AcceptanceNumber = parameter.AcceptanceNumber.Value;

            if (parameter.OpenPdf)
            {
                var pdf = await _transactionContext.CreateLabelsForAcceptanceAsync(_context.Basar, parameter.AcceptanceNumber.Value);
                return File(pdf.Data, pdf.ContentType);
            }

            OpenPdf = true;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!await _transactionContext.ExistsAsync(_context.Basar, TransactionType.Acceptance, AcceptanceNumber))
            {
                NumberNotFound = true;
                return Page();
            }
            return this.RedirectToPage<SelectAcceptanceModel>(new SelectAcceptanceParameter { AcceptanceNumber = AcceptanceNumber });
        }
        public string GetOpenPdfPage() => _linkGenerator.GetPath(this.GetPage<CreateAndPrintForAcceptanceModel>(new CreateAndPrintForAcceptanceParameter { AcceptanceNumber = AcceptanceNumber }));
    }
}