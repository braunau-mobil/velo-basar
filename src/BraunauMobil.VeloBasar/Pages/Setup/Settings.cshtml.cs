using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class SettingsModel : PageModel
    {
        private readonly VeloBasarContext _context;

        public SettingsModel(VeloBasarContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Display(Name = "Aktiver Basar")]
        public int ActiveBasarId { get; set; }

        public void OnGet()
        {
            ViewData["Basars"] = new SelectList(_context.Basar, "Id", "Name");
            ActiveBasarId = _context.Settings.ActiveBasar.Id;
        }
        public async Task OnPostAsync()
        {
            ViewData["Basars"] = new SelectList(_context.Basar, "Id", "Name");
            _context.Settings.ActiveBasarId = ActiveBasarId;
            await _context.SaveChangesAsync();
        }
    }
}