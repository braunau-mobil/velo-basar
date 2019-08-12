using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages
{
    public class ShowPdfModel : BasarPageModel
    {
        public ShowPdfModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, string path)
        {
            await LoadBasarAsync(basarId);

            if (string.IsNullOrEmpty(path))
            {
                return NotFound();
            }

            return Redirect(path);
        }
    }
}