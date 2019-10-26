using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class CreateModel : BasarPageModel
    {
        public CreateModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public ProductType ProductType { get; set; }

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

            await Context.CreateProductType(ProductType);
            return RedirectToPage("/ProductTypes/List", GetRoute());
        }
    }
}