using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages
{
    public class DangerZoneModel : BasarPageModel
    {
        public DangerZoneModel(VeloBasarContext context) : base(context)
        {
            Config = new DataGeneratorConfiguration();
        }

        [BindProperty]
        public DataGeneratorConfiguration Config { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var generator = new DataGenerator(Context, Config);
            await generator.GenerateAsync();

            return Page();
        }
    }
}