using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class CreateModel : BasarPageModel
    {
        public CreateModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Seller Seller { get; set; }

        public async Task OnGetAsync(int? basarId)
        {
            await LoadBasarAsync(basarId);

            ViewData["Countries"] = new SelectList(Context.Country, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Seller.Add(Seller);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Sellers/Details", new { basarId = Basar.Id, sellerId = Seller.Id });
        }
    }
}