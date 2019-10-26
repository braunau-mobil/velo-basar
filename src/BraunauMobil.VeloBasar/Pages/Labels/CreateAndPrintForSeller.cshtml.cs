using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class CreateAndPrintForSellerModel : BasarPageModel
    {
        public CreateAndPrintForSellerModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int sellerId)
        {
            await LoadBasarAsync(basarId);

            var file = await Context.CreateLabelsForSellerAsync(Basar, sellerId);

            return File(file.Data, file.ContentType);
        }
    }
}