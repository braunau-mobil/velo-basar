using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
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
        public VeloSettings VeloSettings { get; set; }
        [BindProperty]
        [Display(Name = "WordPress Verkaufs-Status Push")]
        public WordPressStatusPushSettings WordPressStatusPushSettings { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["Basars"] = _basarContext.GetSelectList();

            VeloSettings = await _settingsContext.GetSettingsAsync();
            WordPressStatusPushSettings = VeloSettings.WordPressStatusPushSettings;
        }
        public async Task OnPostAsync()
        {
            ViewData["Basars"] = _basarContext.GetSelectList();

            VeloSettings.WordPressStatusPushSettings = WordPressStatusPushSettings;
            
            await _settingsContext.UpdateAsync(VeloSettings);
        }
    }
}