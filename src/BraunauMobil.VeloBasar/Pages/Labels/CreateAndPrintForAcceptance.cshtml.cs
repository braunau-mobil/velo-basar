using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class CreateAndPrintForAcceptanceModel : BasarPageModel
    {
        public CreateAndPrintForAcceptanceModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int acceptanceNumber)
        {
            await LoadBasarAsync(basarId);
            var pdf = await Context.CreateLabelsForAcceptanceAsync(Basar, acceptanceNumber);
            return File(pdf.Data, pdf.ContentType);
        }
    }
}