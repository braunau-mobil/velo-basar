using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    [Authorize]
    public class SetActiveBasarModel : PageModel
    {
        private readonly ISettingsContext _context;

        public SetActiveBasarModel(ISettingsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int basarId)
        {
            var settings = await _context.GetSettingsAsync();
            settings.ActiveBasarId = basarId;
            await _context.UpdateAsync(settings);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}