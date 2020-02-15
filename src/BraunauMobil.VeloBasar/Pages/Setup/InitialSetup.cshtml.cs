using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class InitialSetupModel : PageModel
    {
        private readonly ISetupContext _context;

        public InitialSetupModel(ISetupContext context)
        {
            _context = context;
            Config = new InitializationConfiguration();
        }

        [BindProperty]
        public InitializationConfiguration Config
        {
            get;
            set;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.CreateDatabaseAsync();
            await _context.InitializeDatabaseAsync(Config);

            return this.RedirectToPage<IndexModel>();
        }
    }
}