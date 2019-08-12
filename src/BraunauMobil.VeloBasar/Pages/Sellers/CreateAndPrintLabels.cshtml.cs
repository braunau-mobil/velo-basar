using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Sellers
{
    public class CreateAndPrintLabelsModel : BasarPageModel
    {
        public CreateAndPrintLabelsModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            await Context.GenerateMissingLabelsAsync(basarId, sellerId);

            var file = await Context.GetAllLabelsAsyncAsCombinedPdf(basarId, sellerId);

            return File(file.Data, file.ContentType);
        }
    }
}