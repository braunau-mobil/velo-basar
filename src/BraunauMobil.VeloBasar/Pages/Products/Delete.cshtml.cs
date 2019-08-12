using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Products
{
    public class DeleteModel : BasarPageModel
    {
        public DeleteModel(VeloBasarContext context) : base(context)
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

            if (!await Context.DeleteProductAsync(productId))
            {
                return RedirectToPage("/Dialogs/Error", new
                {
                    basarId,
                    targetPage = Request.Headers["Referer"],
                    message = "Der Verkäufer konnte nicht gelöscht werden."
                });
            }

            return Redirect(target);
        }
    }
}
