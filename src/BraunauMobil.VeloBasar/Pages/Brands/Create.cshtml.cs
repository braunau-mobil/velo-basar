using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class CreateModel : BasarPageModel
    {
        public CreateModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Brand Brand { get; set; }

        public async Task OnGetTask(int? basarId)
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

            await Context.CreateBrand(Brand);
            return RedirectToPage("/Brands/List", GetRoute());
        }
    }
}