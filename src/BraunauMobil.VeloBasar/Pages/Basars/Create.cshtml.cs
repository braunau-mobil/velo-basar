using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Models;
using System;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    public class CreateModel : BasarPageModel
    {
        public CreateModel(VeloBasarContext context) : base(context)
        {
            Basar = new Basar
            {
                Date = DateTime.Now
            };
        }

        [BindProperty]
        public Basar BasarToCreate { get; set; }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);
        }
        public async Task<IActionResult> OnPostAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await Context.CreateBasarAsync(BasarToCreate);

            return RedirectToPage("/Basars/List", new { basarId = Basar.Id });
        }
    }
}