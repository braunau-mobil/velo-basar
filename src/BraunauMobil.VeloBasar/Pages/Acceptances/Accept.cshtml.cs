using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class AcceptModel : BasarPageModel
    {
        public AcceptModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            var products = Request.Cookies.GetAcceptanceProducts();
            var acceptance = await Context.AcceptProductsAsync(Basar, sellerId, products);

            Response.Cookies.ClearAcceptanceProducts();
            return RedirectToPage("/Acceptances/Details", new { basarId, acceptanceId = acceptance.Id, showSuccess = true, openDocument = true });
        }
    }
}