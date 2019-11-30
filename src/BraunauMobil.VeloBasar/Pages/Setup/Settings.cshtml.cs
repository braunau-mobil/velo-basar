using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class SettingsModel : PageModel
    {
        private readonly IVeloContext _context;

        public SettingsModel(IVeloContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Display(Name = "Aktiver Basar")]
        public int ActiveBasarId { get; set; }

        public void OnGet()
        {
            ViewData["Basars"] = new SelectList(_context.Db.Basar, "Id", "Name");
            ActiveBasarId = _context.Settings.ActiveBasarId.Value;
        }
        public async Task OnPostAsync()
        {
            ViewData["Basars"] = new SelectList(_context.Db.Basar, "Id", "Name");
            _context.Settings.ActiveBasarId = ActiveBasarId;
            await _context.Db.SaveChangesAsync();
        }
    }
}