using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class SettingsModel : PageModel
    {
        private readonly ISettingsContext _settingsContext;
        private readonly IBasarContext _basarContext;

        public SettingsModel(ISettingsContext settingsContext, IBasarContext basarContext)
        {
            _settingsContext = settingsContext;
            _basarContext = basarContext;
        }

        [BindProperty]
        [Display(Name = "Aktiver Basar")]
        public int ActiveBasarId { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["Basars"] = _basarContext.GetSelectList();

            var settings = await _settingsContext.GetSettingsAsync();
            if (settings.ActiveBasarId.HasValue)
            {
                ActiveBasarId = settings.ActiveBasarId.Value;
            }
        }
        public async Task OnPostAsync()
        {
            ViewData["Basars"] = _basarContext.GetSelectList();

            var settings = await _settingsContext.GetSettingsAsync();
            settings.ActiveBasarId = ActiveBasarId;
            await _settingsContext.UpdateAsync(settings);
        }
    }
}