using System.IO;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class PrintSettingsModel : PageModel
    {
        private readonly ISettingsContext _context;

        public PrintSettingsModel(ISettingsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PrintSettings PrintSettings { get; set; }
        [BindProperty]
        public AcceptancePrintSettings Acceptance { get; set; }
        [BindProperty]
        public SalePrintSettings Sale { get; set; }
        [BindProperty]
        public Margins PageMargins { get; set; }
        [BindProperty]
        public IFormFile BannerUpload { get; set; }

        public async Task OnGet()
        {
            PrintSettings = await _context.GetPrintSettingsAsync();
            Acceptance = PrintSettings.Acceptance;
            Sale = PrintSettings.Sale;
            PageMargins = PrintSettings.PageMargins;
        }
        public async Task OnPostAsync()
        {
            await UploadBannerAsync();
            PrintSettings.Acceptance = Acceptance;
            PrintSettings.Sale = Sale;
            PrintSettings.PageMargins = PageMargins;

            await _context.UpdateAsync(PrintSettings);
        }

        public async Task UploadBannerAsync()
        {
            if (BannerUpload == null)
            {
                var currentPrintSettings = await _context.GetPrintSettingsAsync();
                PrintSettings.Banner = currentPrintSettings.Banner;
                return;
            }
            using (var memoryStream = new MemoryStream())
            {
                await BannerUpload.CopyToAsync(memoryStream);
                PrintSettings.Banner = new ImageData
                {
                    Bytes = memoryStream.ToArray(),
                    ImageType = BannerUpload.ContentType
                };
            }
        }
    }
}