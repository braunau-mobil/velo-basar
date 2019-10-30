using System.Threading.Tasks;
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

        public CreateAndPrintForAcceptanceModel(IVeloContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(CreateAndPrintForAcceptanceParameter parameter)
        {
            var pdf = await _context.Db.CreateLabelsForAcceptanceAsync(_context.Basar, parameter.AcceptanceNumber);
            return File(pdf.Data, pdf.ContentType);
        }
    }
}