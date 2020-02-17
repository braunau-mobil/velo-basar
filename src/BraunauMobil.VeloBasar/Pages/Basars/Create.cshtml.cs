using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class CreateModel : PageModel
    {
        private readonly IBasarContext _basarContext;

        public CreateModel(IBasarContext basarContext)
        {
            _basarContext = basarContext;
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

            await _basarContext.CreateAsync(BasarToCreate);
            return this.RedirectToPage<ListModel>();
        }
    }
}