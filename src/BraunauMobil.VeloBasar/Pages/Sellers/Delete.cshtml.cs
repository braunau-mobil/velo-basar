using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class DeleteModel : BasarPageModel
    {
        private string _sourcePage;

        public DeleteModel(VeloBasarContext context) : base(context)
        {
        }

        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId, string sourcePage)
        {
            await LoadBasarAsync(basarId);

            Seller = await Context.Seller.FirstOrDefaultAsync(m => m.Id == sellerId);

            if (Seller == null)
            {
                return NotFound();
            }

            _sourcePage = sourcePage;

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
                    targetPage = _sourcePage,
                    message = "Der Verkäufer konnte nicht gelöscht werden."
                });
            }
           
            return RedirectToPage("./List", new { basarId = Basar.Id });
        }

        public string GetCancelPage()
        {
            return $"./{_sourcePage}";
        }

        public IDictionary<string, string> GetCancelRoute()
        {
            var route = GetRoute();

            if (_sourcePage == "Details")
            {
                route.Add("sellerId", Seller.Id.ToString());
            }

            return route;
        }
    }
}
