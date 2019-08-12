using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class EditModel : BasarPageModel
    {
        private string _cancelPage;

        public EditModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId, string sourcePage)
        {
            await LoadBasarAsync(basarId);

            ViewData["Countries"] = new SelectList(Context.Country, "Id", "Name");

            Seller = await Context.Seller.FirstOrDefaultAsync(m => m.Id == sellerId);

            if (Seller == null)
            {
                return NotFound();
            }

            _cancelPage = sourcePage;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int basarId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await LoadBasarAsync(basarId);

            Context.Attach(Seller).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Context.Seller.ExistsAsync(Seller.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { basarId, sellerId = Seller.Id });
        }

        public string GetCancelPage()
        {
            return $"./{_cancelPage}";
        }

        public IDictionary<string, string> GetCancelRoute()
        {
            var route = GetRoute();

            if (_cancelPage == "Details")
            {
                route.Add("sellerId", Seller.Id.ToString());
            }

            return route;
        }
    }
}
