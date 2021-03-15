using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.DevTools
{
    public class SellerExportModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly IExportContext _exportService;

        public string ExportPath { get; set; }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {


            return Page();
        }
    }
}
