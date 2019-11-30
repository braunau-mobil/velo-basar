using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System;

namespace BraunauMobil.VeloBasar.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly ILogger _logger = Log.ForContext<IndexModel>();

        public IndexModel(IVeloContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            _logger.Information("OnGet");

            //  check if we need initial setup
            if (!_context.Db.IsInitialized())
            {
                _logger.Information("DB not initialized");
                return this.RedirectToPage<Setup.InitialSetupModel>();
            }

            if (_context.Basar == null)
            {
                _logger.Information("No basar found");
                if (_context.Configuration.DevToolsEnabled())
                {
                    return this.RedirectToPage<DevTools.DangerZoneModel>();
                }
                return this.RedirectToPage<Setup.LoginModel>();
            }
            return this.RedirectToPage<Basars.DetailsModel>(new Basars.DetailsParameter());
        }
    }
}
