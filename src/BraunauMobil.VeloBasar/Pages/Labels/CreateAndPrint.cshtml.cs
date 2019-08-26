using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class CreateAndPrintModel : BasarPageModel
    {
        public CreateAndPrintModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public Seller Seller { get; set; }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            await Context.GenerateMissingLabelsAsync(Basar, sellerId);

            var file = await Context.GetAllLabelsAsyncAsCombinedPdf(Basar, sellerId);

            return File(file.Data, file.ContentType);
        }
    }
}