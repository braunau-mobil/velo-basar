using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
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


        public CreateAndPrintModel(IVeloContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(CreateAndPrintParameter parameter)
        {
            var printSettings = await _context.Db.GetPrintSettingsAsync();
            var settlement = await _context.Db.SettleSellerAsync(_context.Basar, parameter.SellerId, printSettings);
            var file = await _context.Db.FileStore.GetAsync(settlement.DocumentId.Value);

            return File( file.Data, file.ContentType);
        }
    }
}