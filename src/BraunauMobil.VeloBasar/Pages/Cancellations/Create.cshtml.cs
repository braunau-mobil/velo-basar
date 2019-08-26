using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Cancellations
{
    public class CreateModel : BasarPageModel
    {
        public CreateModel(VeloBasarContext context) : base(context)
        {
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int productId)
        {
            await LoadBasarAsync(basarId);

            Product = await Context.Product.FirstOrDefaultAsync(p => p.Id == productId);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int productId, string target)
        {
            await LoadBasarAsync(basarId);

            await Context.CancelProductAsync(Basar, productId);

            return Redirect(target);
        }
    }
}
