using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages
{
    public class ShowPdfModel : BasarPageModel
    {
        public ShowPdfModel(VeloBasarContext context) : base(context)
        {
        }

        public async Task<IActionResult> OnGetAsync(int basarId, int? acceptanceId)
        {
            await LoadBasarAsync(basarId);

            string pdfPath = "";
            if (acceptanceId != null)
            {
                pdfPath = await Context.GetAcceptancePdfAsync(acceptanceId.Value);
            }

            if (string.IsNullOrEmpty(pdfPath))
            {
                return NotFound();
            }

            return Redirect(pdfPath);
        }
    }
}