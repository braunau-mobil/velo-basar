using System;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class PreviewModel : PageModel
    {
        private readonly ISettingsContext _settingsContext;
        private readonly IPrintService _printService;

        public PreviewModel(ISettingsContext settingsContext, IPrintService printService)
        {
            _settingsContext = settingsContext;
            _printService = printService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _settingsContext.GetPrintSettingsAsync();
            var bytes = _printService.CreateLabel(SampleProduct, settings);
            return File(bytes, "application/pdf");
        }

        private static Product SampleProduct => new Product
        {
            Basar = new Basar
            {
                Date = new DateTime(2063, 04, 05),
                Id = 1,
                Name = "Fahrradbasar",
                Location = "Hopfenhausen",
                ProductCommission = 0.1m
            },
            Brand = new Brand
            {
                Name = "Marke"
            },
            Color = "Grün Blau Rot",
            Description = "Gepäcksträger, Licht, Korb",
            FrameNumber = "123-456-789",
            Id = 223,
            Price = 123.23m,
            TireSize = "38\"",
            Type = new ProductType
            {
                Name = "Lastenrad"
            }
        };
    }
}