using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class SalePrintSettingsViewModel
    {
        public SalePrintSettingsViewModel()
        {
            Settings = new SalePrintSettings();
        }

        [BindProperty]
        public SalePrintSettings Settings { get; set; }
        [BindProperty]
        public IFormFile BannerUpload { get; set; }

        public async Task UploadBannerAsync()
        {
            if (BannerUpload == null)
            {
                return;
            }
            using (var memoryStream = new MemoryStream())
            {
                await BannerUpload.CopyToAsync(memoryStream);
                Settings.Banner = new ImageData
                {
                    Bytes = memoryStream.ToArray(),
                    ImageType = BannerUpload.ContentType
                };
            }
        }
    }
}
