using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    [Authorize]
    public class SetActiveBasarModel : PageModel
    {
        private readonly IVeloContext _context;

        public SetActiveBasarModel(IVeloContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int basarId)
        {
            _context.Settings.ActiveBasarId = basarId;
            await _context.SaveSettingsAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}