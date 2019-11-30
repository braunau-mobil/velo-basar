using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class CreateModel : PageModel
    {
        private readonly IVeloContext _context;

        public CreateModel(IVeloContext context)
        {
            _context = context;
            BasarToCreate = new Basar
            {
                Date = DateTime.Now
            };
        }

        [BindProperty]
        public Basar BasarToCreate { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var basar = await _context.Db.CreateBasarAsync(BasarToCreate);
            if (_context.Db.Basar.Count() == 1)
            {
                _context.Settings.ActiveBasarId = basar.Id;
                await _context.SaveSettingsAsync();
            }

            return this.RedirectToPage<ListModel>();
        }
    }
}