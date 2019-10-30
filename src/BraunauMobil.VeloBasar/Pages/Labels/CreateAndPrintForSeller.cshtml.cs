using System.Threading.Tasks;
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

        public CreateAndPrintForSellerModel(IVeloContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(CreateAndPrintForSellerParameter parameter)
        {
            var file = await _context.Db.CreateLabelsForSellerAsync(_context.Basar, parameter.SellerId);

            return File(file.Data, file.ContentType);
        }
    }
}