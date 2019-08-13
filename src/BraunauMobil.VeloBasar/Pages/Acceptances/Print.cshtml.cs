using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Acceptances
{
    public class PrintModel : BasarPageModel
    {
        public PrintModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int acceptanceId)
        {
            await LoadBasarAsync(basarId);

            var doc = await Context.GenerateAcceptanceDocIfNotExistAsync(Basar, acceptanceId);

            return File(doc.Data, doc.ContentType);
        }
    }
}