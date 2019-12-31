using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.DevTools
{
    public class DangerZoneModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly IDataGeneratorContext _generatorContext;

        public DangerZoneModel(IVeloContext context, IDataGeneratorContext generatorContext)
        {
            _context = context;
            _generatorContext = generatorContext;
            Config = new DataGeneratorConfiguration
            {
                GenerateBrands = true,
                GenerateCountries = true,
                GenerateProductTypes = true
            };
        }

        [BindProperty]
        public DataGeneratorConfiguration Config { get; set; }

        public IActionResult OnGet()
        {
            if (!_context.DevToolsEnabled())
            {
                return Unauthorized();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!_context.DevToolsEnabled())
            {
                return Unauthorized();
            }
            await _generatorContext.GenerateAsync(Config);

            return this.RedirectToPage<IndexModel>();
        }
    }
}
