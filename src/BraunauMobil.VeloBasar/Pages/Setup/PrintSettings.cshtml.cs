using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class PrintSettingsModel : PageModel
    {
        private readonly IVeloContext _context;

        public PrintSettingsModel(IVeloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AcceptancePrintSettings Acceptance { get; set; }
        [BindProperty]
        public SalePrintSettingsViewModel Sale { get; set; }
        [BindProperty]
        public Margins PageMargins { get; set; }

        public async Task OnGet()
        {
            var printSettings = await _context.Db.GetPrintSettingsAsync();
            Acceptance = printSettings.Acceptance;
            Sale = new SalePrintSettingsViewModel { Settings = printSettings.Sale };
            PageMargins = printSettings.PageMargins;
        }
        public async Task OnPostAsync()
        {
            await Sale.UploadBannerAsync();

            var printSettings = await _context.Db.GetPrintSettingsAsync();
            printSettings.Acceptance = Acceptance;
            printSettings.Sale = Sale.Settings;
            printSettings.PageMargins = PageMargins;

            await _context.Db.SetPrintSettingsAsync(printSettings);
        }
    }
}