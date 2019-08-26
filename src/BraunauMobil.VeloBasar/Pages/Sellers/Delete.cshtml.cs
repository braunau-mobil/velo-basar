using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class DeleteModel : BasarPageModel
    {
        public DeleteModel(VeloBasarContext context) : base(context)
        {
        }

        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            Seller = await Context.Seller.FirstOrDefaultAsync(m => m.Id == sellerId);

            if (Seller == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            if (!await Context.DeleteSellerAsync(sellerId))
            {
                return RedirectToPage("/Dialogs/Error", new
                {
                    basarId,
                    sellerId,
                    targetPage = Request.Headers["Referer"],
                    message = "Der Verkäufer konnte nicht gelöscht werden."
                });
            }
           
            return RedirectToPage("/Sellers/List", GetRoute());
        }
    }
}
