using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages
{
    public class DangerZoneModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public DangerZoneModel(VeloBasarContext context)
        {
            _context = context;
            Config = new DataGeneratorConfiguration();
        }

        [BindProperty]
        public DataGeneratorConfiguration Config { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var generator = new DataGenerator(_context, Config);
            await generator.GenerateAsync();

            return Page();
        }
    }
}