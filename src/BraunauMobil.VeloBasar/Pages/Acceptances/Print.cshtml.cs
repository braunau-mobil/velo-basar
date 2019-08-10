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

        public async Task<IActionResult> OnGetAsync(int acceptanceId)
        {
            var printResultPath = await Context.PrintAcceptanceAsync(acceptanceId);

            return Redirect(printResultPath);

        }
    }
}