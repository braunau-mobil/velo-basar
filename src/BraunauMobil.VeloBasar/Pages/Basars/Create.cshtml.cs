using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class CreateModel : BasarPageModel
    {
        public CreateModel(BraunauMobil.VeloBasar.Data.VeloBasarContext context) : base(context)
        {
            Basar = new Basar
            {
                Date = DateTime.Now
            };
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Basar.Add(Basar);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Index", new { basarId = Basar.Id });
        }
    }
}