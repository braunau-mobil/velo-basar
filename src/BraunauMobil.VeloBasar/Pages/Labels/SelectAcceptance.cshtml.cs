using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
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
        private readonly IFileStoreContext _fileStoreContext;

        public SelectAcceptanceModel(IVeloContext context, LinkGenerator linkGenerator, ITransactionContext transactionContext, IFileStoreContext fileStoreContext)
        {
            _context = context;
            _linkGenerator = linkGenerator;
            _transactionContext = transactionContext;
            _fileStoreContext = fileStoreContext;
        }

        [BindProperty]
        [Display(Name = "Annahme Nummer")]
        public int AcceptanceNumber { get; set; }
        public bool NumberNotFound { get; set; }
        public bool OpenPdf { get; set; }

        public async Task<IActionResult> OnGetAsync(SelectAcceptanceParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (parameter.AcceptanceNumber == null)
            {
                return Page();
            }

            AcceptanceNumber = parameter.AcceptanceNumber.Value;

            if (parameter.OpenPdf)
            {
                var acceptance = await _transactionContext.GetAsync(_context.Basar, TransactionType.Acceptance, AcceptanceNumber);
                var pdf = await _fileStoreContext.GetProductLabelsAndCombineToOnePdfAsync(acceptance.Products.GetProducts());
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
        public string GetOpenPdfPage() => _linkGenerator.GetPath(this.GetPage<PrintForAcceptanceModel>(new PrintForAcceptanceParameter { AcceptanceNumber = AcceptanceNumber }));
    }
}